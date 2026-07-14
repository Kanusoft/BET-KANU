using BET_KANU.Services;
using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using BetKanu.Models.ViewModels;
using BetKanu.Models.ViewModels.MagazineArticles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class MagazineManagerController : Controller
    {
        private const long MaximumImageSize = 5 * 1024 * 1024;
        private const long MaximumArticlePdfSize = 25 * 1024 * 1024;

        private static readonly string[] AllowedImageExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private static readonly string[] AllowedPdfExtensions =
        {
            ".pdf"
        };

        private static readonly string[] AllowedPdfMimeTypes =
        {
            "application/pdf",
            "application/octet-stream"
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorage;
        private readonly ILogger<MagazineManagerController> _logger;

        public MagazineManagerController(
            IUnitOfWork unitOfWork,
            IBlobStorageService blobStorage,
            ILogger<MagazineManagerController> logger)
        {
            _unitOfWork = unitOfWork;
            _blobStorage = blobStorage;
            _logger = logger;
        }

        // =====================================================
        // MAGAZINES
        // =====================================================

        // LIST
        // =====================================================

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var magazines = _unitOfWork.Magazines
                    .GetAll()
                    .Select(m => new MagazineListItemViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Slug = m.Slug,
                        Year = m.Year,
                        DisplayOrder = m.DisplayOrder,
                        CoverImage = m.CoverImage,
                        IsPublished = m.IsPublished,
                        PublishedAtUtc = m.PublishedAtUtc,
                        CreatedAtUtc = m.CreatedAtUtc,
                        ArticleCount = m.Articles?.Count ?? 0
                    })
                    .ToList();

                var viewModel = new MagazineIndexViewModel
                {
                    Magazines = magazines
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading magazines.");

                TempData["ErrorMessage"] =
                    "The magazines could not be loaded. Please try again.";

                return View(new MagazineIndexViewModel());
            }
        }

        // =====================================================
        // CREATE
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MagazineFormViewModel
            {
                Year = DateTime.UtcNow.Year,
                DisplayOrder = 0,
                IsPublished = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MagazineFormViewModel model)
        {
            NormalizeForm(model);

            ValidateCoverImage(model.CoverImageFile);

            ValidatePublication(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string? coverImagePath = null;

                if (model.CoverImageFile != null)
                {
                    coverImagePath = await SaveImage(
                        model.CoverImageFile);
                }

                var magazine = new Magazine
                {
                    Title = model.Title,
                   
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    Year = model.Year,
                    DisplayOrder = model.DisplayOrder,
                    CoverImage = coverImagePath,
                    IsPublished = model.IsPublished,
                    PublishedAtUtc = model.IsPublished
                        ? model.PublishedAtUtc ?? DateTime.UtcNow
                        : null,
                    CreatedAtUtc = DateTime.UtcNow
                };

                _unitOfWork.Magazines.Add(magazine);

                var result = _unitOfWork.Save();

                if (result <= 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "The magazine could not be saved.");

                    return View(model);
                }

                TempData["SuccessMessage"] =
                    $"Magazine “{magazine.Title}” was created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating magazine {Title}.",
                    model.Title);

                ModelState.AddModelError(
                    string.Empty,
                    "An unexpected error occurred while creating the magazine.");

                return View(model);
            }
        }

        // =====================================================
        // EDIT
        // =====================================================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The requested magazine is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var magazine = _unitOfWork.Magazines.GetById(id);

                if (magazine == null)
                {
                    TempData["ErrorMessage"] =
                        "The requested magazine was not found.";

                    return RedirectToAction(nameof(Index));
                }

                var model = new MagazineFormViewModel
                {
                    Id = magazine.Id,
                    Title = magazine.Title,                 
                    ShortDescription = magazine.ShortDescription,
                    LongDescription = magazine.LongDescription,
                    Year = magazine.Year,
                    DisplayOrder = magazine.DisplayOrder,
                    IsPublished = magazine.IsPublished,
                    PublishedAtUtc = magazine.PublishedAtUtc,
                    ExistingCoverImage = magazine.CoverImage
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading magazine {MagazineId}.",
                    id);

                TempData["ErrorMessage"] =
                    "The magazine could not be loaded.";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            MagazineFormViewModel model)
        {
            if (id != model.Id || id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The magazine request is invalid.";

                return RedirectToAction(nameof(Index));
            }

            NormalizeForm(model);

            ValidateCoverImage(model.CoverImageFile);

            ValidatePublication(model);

            var magazine = _unitOfWork.Magazines.GetById(id);

            if (magazine == null)
            {
                TempData["ErrorMessage"] =
                    "The magazine no longer exists.";

                return RedirectToAction(nameof(Index));
            }

            model.ExistingCoverImage = magazine.CoverImage;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                magazine.Title = model.Title;
               
                magazine.ShortDescription = model.ShortDescription;
                magazine.LongDescription = model.LongDescription;
                magazine.Year = model.Year;
                magazine.DisplayOrder = model.DisplayOrder;
                magazine.IsPublished = model.IsPublished;
                magazine.UpdatedAtUtc = DateTime.UtcNow;

                if (model.IsPublished)
                {
                    magazine.PublishedAtUtc =
                        model.PublishedAtUtc ??
                        magazine.PublishedAtUtc ??
                        DateTime.UtcNow;
                }
                else
                {
                    magazine.PublishedAtUtc = null;
                }

                if (model.CoverImageFile != null)
                {
                    magazine.CoverImage = await SaveImage(
                        model.CoverImageFile);
                }

                _unitOfWork.Magazines.Update(magazine);

                var result = _unitOfWork.Save();

                if (result <= 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No changes were saved.");

                    model.ExistingCoverImage =
                        magazine.CoverImage;

                    return View(model);
                }

                TempData["SuccessMessage"] =
                    $"Magazine “{magazine.Title}” was updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating magazine {MagazineId}.",
                    id);

                ModelState.AddModelError(
                    string.Empty,
                    "An unexpected error occurred while updating the magazine.");

                model.ExistingCoverImage =
                    magazine.CoverImage;

                return View(model);
            }
        }

        // =====================================================
        // DELETE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The magazine request is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var magazine =
                    _unitOfWork.Magazines.GetByIdWithArticles(id);

                if (magazine == null)
                {
                    TempData["ErrorMessage"] =
                        "The magazine was not found.";

                    return RedirectToAction(nameof(Index));
                }

                if (magazine.Articles.Any())
                {
                    TempData["ErrorMessage"] =
                        $"Magazine “{magazine.Title}” cannot be deleted because it contains {magazine.Articles.Count} article(s). Delete its articles first.";

                    return RedirectToAction(nameof(Index));
                }

                var title = magazine.Title;

                _unitOfWork.Magazines.Remove(magazine);

                var result = _unitOfWork.Save();

                if (result <= 0)
                {
                    TempData["ErrorMessage"] =
                        "The magazine could not be deleted.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] =
                    $"Magazine “{title}” was deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting magazine {MagazineId}.",
                    id);

                TempData["ErrorMessage"] =
                    "The magazine could not be deleted. It may still be referenced by other records.";
            }

            return RedirectToAction(nameof(Index));
        }

        // =====================================================
        // ARTICLES
        // =====================================================

        [HttpGet]
        public IActionResult Articles(int magazineId)
        {
            if (magazineId <= 0)
            {
                TempData["ErrorMessage"] =
                    "The requested magazine is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var magazine = _unitOfWork.Magazines.GetById(magazineId);

                if (magazine == null)
                {
                    TempData["ErrorMessage"] =
                        "The requested magazine was not found.";

                    return RedirectToAction(nameof(Index));
                }

                var articles = _unitOfWork.MagazineArticles
                    .GetByMagazineId(magazineId)
                    .Select(a => new MagazineArticleListItemViewModel
                    {
                        Id = a.Id,
                        MagazineId = a.MagazineId,
                        Title = a.Title,
                        Slug = a.Slug,
                        BannerImage = a.BannerImage,
                        ArticleNumber = a.ArticleNumber,
                        DisplayOrder = a.DisplayOrder,
                        ReleaseDate = a.ReleaseDate,
                        Status = a.Status,
                        PublishAtUtc = a.PublishAtUtc,
                        CreatedAtUtc = a.CreatedAtUtc,
                        UpdatedAtUtc = a.UpdatedAtUtc
                    })
                    .ToList();

                var viewModel = new MagazineArticlesIndexViewModel
                {
                    MagazineId = magazine.Id,
                    MagazineTitle = magazine.Title,
                    MagazineSlug = magazine.Slug,
                    MagazineIsPublished = magazine.IsPublished,
                    Articles = articles
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading articles for magazine {MagazineId}.",
                    magazineId);

                TempData["ErrorMessage"] =
                    "The articles could not be loaded. Please try again.";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult CreateArticle(int magazineId)
        {
            if (magazineId <= 0)
            {
                TempData["ErrorMessage"] =
                    "The requested magazine is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var magazine = _unitOfWork.Magazines.GetById(magazineId);

                if (magazine == null)
                {
                    TempData["ErrorMessage"] =
                        "The requested magazine was not found.";

                    return RedirectToAction(nameof(Index));
                }

                var existingArticles = _unitOfWork.MagazineArticles
                    .GetByMagazineId(magazineId)
                    .ToList();

                var nextArticleNumber = existingArticles.Any()
                    ? existingArticles.Max(a => a.ArticleNumber) + 1
                    : 1;

                return View(new MagazineArticleFormViewModel
                {
                    MagazineId = magazine.Id,
                    MagazineTitle = magazine.Title,
                    ArticleNumber = nextArticleNumber,
                    DisplayOrder = nextArticleNumber,
                    ReleaseDate = DateTime.UtcNow.Date,
                    Status = MagazineArticleStatus.Draft
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while preparing create article for magazine {MagazineId}.",
                    magazineId);

                TempData["ErrorMessage"] =
                    "The article form could not be loaded.";

                return RedirectToAction(
                    nameof(Articles),
                    new { magazineId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(
            MagazineArticleFormViewModel model)
        {
            var magazine = _unitOfWork.Magazines.GetById(model.MagazineId);

            if (magazine == null)
            {
                TempData["ErrorMessage"] =
                    "The requested magazine was not found.";

                return RedirectToAction(nameof(Index));
            }

            model.MagazineTitle = magazine.Title;

            NormalizeArticleForm(model);
            ValidateArticleNumber(model);
            ValidateArticleBanner(model.BannerImageFile);
            ValidateArticlePdf(
                model.WesternPdfFile,
                nameof(MagazineArticleFormViewModel.WesternPdfFile),
                "Western PDF");
            ValidateArticlePdf(
                model.EasternPdfFile,
                nameof(MagazineArticleFormViewModel.EasternPdfFile),
                "Eastern PDF");
            ValidateArticlePublication(model);
            ValidateArticleContent(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                string? bannerImagePath = null;
                string? westernPdfPath = null;
                string? easternPdfPath = null;

                if (model.BannerImageFile != null)
                {
                    bannerImagePath = await SaveArticleImage(
                        model.BannerImageFile);
                }

                if (model.WesternPdfFile != null)
                {
                    westernPdfPath = await SaveArticlePdf(
                        model.WesternPdfFile,
                        "magazine/articles/pdfs/western");
                }

                if (model.EasternPdfFile != null)
                {
                    easternPdfPath = await SaveArticlePdf(
                        model.EasternPdfFile,
                        "magazine/articles/pdfs/eastern");
                }

                var article = new MagazineArticle
                {
                    MagazineId = model.MagazineId,
                    Title = model.Title,
                    ShortDescription = model.ShortDescription,
                    ArticleNumber = model.ArticleNumber,
                    DisplayOrder = model.DisplayOrder,
                    ReleaseDate = model.ReleaseDate,
                    BannerImage = bannerImagePath,
                    WesternTitle = model.WesternTitle,
                    WesternIntroduction = model.WesternIntroduction,
                    WesternBodyHtml = model.WesternBodyHtml,
                    WesternCreditsHtml = model.WesternCreditsHtml,
                    WesternPdfPath = westernPdfPath,
                    EasternTitle = model.EasternTitle,
                    EasternIntroduction = model.EasternIntroduction,
                    EasternBodyHtml = model.EasternBodyHtml,
                    EasternCreditsHtml = model.EasternCreditsHtml,
                    EasternPdfPath = easternPdfPath,
                    Status = model.Status,
                    CreatedAtUtc = DateTime.UtcNow
                };

                ApplyArticlePublication(article, model);

                _unitOfWork.MagazineArticles.Add(article);

                var result = _unitOfWork.Save();

                if (result <= 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "The article could not be saved.");

                    return View(model);
                }

                TempData["SuccessMessage"] =
                    $"Article “{article.Title}” was created successfully.";

                return RedirectToAction(
                    nameof(Articles),
                    new { magazineId = article.MagazineId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "A database error occurred while creating article {Title} for magazine {MagazineId}.",
                    model.Title,
                    model.MagazineId);

                ModelState.AddModelError(
                    string.Empty,
                    "The article could not be saved because of a database conflict.");

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while creating article {Title} for magazine {MagazineId}.",
                    model.Title,
                    model.MagazineId);

                ModelState.AddModelError(
                    string.Empty,
                    "An unexpected error occurred while creating the article.");

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult EditArticle(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The requested article is invalid.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                var article = _unitOfWork.MagazineArticles
                    .GetByIdWithMagazine(id);

                if (article?.Magazine == null)
                {
                    TempData["ErrorMessage"] =
                        "The requested article was not found.";

                    return RedirectToAction(nameof(Index));
                }

                var model = MapArticleToFormViewModel(article);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while loading article {ArticleId}.",
                    id);

                TempData["ErrorMessage"] =
                    "The article could not be loaded.";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticle(
            int id,
            MagazineArticleFormViewModel model)
        {
            if (id != model.Id || id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The article request is invalid.";

                return RedirectToAction(nameof(Index));
            }

            var article = _unitOfWork.MagazineArticles
                .GetByIdWithMagazine(id);

            if (article?.Magazine == null)
            {
                TempData["ErrorMessage"] =
                    "The requested article was not found.";

                return RedirectToAction(nameof(Index));
            }

            RestoreArticleFormFiles(model, article);
            model.MagazineTitle = article.Magazine.Title;

            NormalizeArticleForm(model);
            ValidateArticleNumber(model);
            ValidateArticleBanner(model.BannerImageFile);
            ValidateArticlePdf(
                model.WesternPdfFile,
                nameof(MagazineArticleFormViewModel.WesternPdfFile),
                "Western PDF");
            ValidateArticlePdf(
                model.EasternPdfFile,
                nameof(MagazineArticleFormViewModel.EasternPdfFile),
                "Eastern PDF");
            ValidateArticlePublication(model);
            ValidateArticleContent(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                article.Title = model.Title;
                article.ShortDescription = model.ShortDescription;
                article.ArticleNumber = model.ArticleNumber;
                article.DisplayOrder = model.DisplayOrder;
                article.ReleaseDate = model.ReleaseDate;
                article.WesternTitle = model.WesternTitle;
                article.WesternIntroduction = model.WesternIntroduction;
                article.WesternBodyHtml = model.WesternBodyHtml;
                article.WesternCreditsHtml = model.WesternCreditsHtml;
                article.EasternTitle = model.EasternTitle;
                article.EasternIntroduction = model.EasternIntroduction;
                article.EasternBodyHtml = model.EasternBodyHtml;
                article.EasternCreditsHtml = model.EasternCreditsHtml;
                article.Status = model.Status;
                article.UpdatedAtUtc = DateTime.UtcNow;

                ApplyArticlePublication(article, model);

                if (model.BannerImageFile != null)
                {
                    article.BannerImage = await SaveArticleImage(
                        model.BannerImageFile);
                }

                if (model.WesternPdfFile != null)
                {
                    article.WesternPdfPath = await SaveArticlePdf(
                        model.WesternPdfFile,
                        "magazine/articles/pdfs/western");
                }

                if (model.EasternPdfFile != null)
                {
                    article.EasternPdfPath = await SaveArticlePdf(
                        model.EasternPdfFile,
                        "magazine/articles/pdfs/eastern");
                }

                _unitOfWork.MagazineArticles.Update(article);

                var result = _unitOfWork.Save();

                TempData["SuccessMessage"] = result > 0
                    ? $"Article “{article.Title}” was updated successfully."
                    : "No changes were detected.";

                return RedirectToAction(
                    nameof(Articles),
                    new { magazineId = article.MagazineId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "A database error occurred while updating article {ArticleId}.",
                    id);

                RestoreArticleFormFiles(model, article);

                ModelState.AddModelError(
                    string.Empty,
                    "The article could not be saved because of a database conflict.");

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while updating article {ArticleId}.",
                    id);

                RestoreArticleFormFiles(model, article);

                ModelState.AddModelError(
                    string.Empty,
                    "An unexpected error occurred while updating the article.");

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteArticle(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] =
                    "The article request is invalid.";

                return RedirectToAction(nameof(Index));
            }

            int magazineId = 0;

            try
            {
                var article = _unitOfWork.MagazineArticles.GetById(id);

                if (article == null)
                {
                    TempData["ErrorMessage"] =
                        "The requested article was not found.";

                    return RedirectToAction(nameof(Index));
                }

                var title = article.Title;
                magazineId = article.MagazineId;

                _unitOfWork.MagazineArticles.Remove(article);

                var result = _unitOfWork.Save();

                if (result <= 0)
                {
                    TempData["ErrorMessage"] =
                        "The article could not be deleted.";

                    return RedirectToAction(
                        nameof(Articles),
                        new { magazineId });
                }

                TempData["SuccessMessage"] =
                    $"Article “{title}” was deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while deleting article {ArticleId}.",
                    id);

                TempData["ErrorMessage"] =
                    "The article could not be deleted.";

                if (magazineId <= 0)
                {
                    var article = _unitOfWork.MagazineArticles.GetById(id);
                    magazineId = article?.MagazineId ?? 0;
                }
            }

            if (magazineId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(
                nameof(Articles),
                new { magazineId });
        }

        // =====================================================
        // ARTICLE HELPERS
        // =====================================================

        private static MagazineArticleFormViewModel MapArticleToFormViewModel(
            MagazineArticle article)
        {
            return new MagazineArticleFormViewModel
            {
                Id = article.Id,
                MagazineId = article.MagazineId,
                MagazineTitle = article.Magazine.Title,
                Title = article.Title,
                ShortDescription = article.ShortDescription,
                ArticleNumber = article.ArticleNumber,
                DisplayOrder = article.DisplayOrder,
                ReleaseDate = article.ReleaseDate,
                ExistingBannerImage = article.BannerImage,
                WesternTitle = article.WesternTitle,
                WesternIntroduction = article.WesternIntroduction,
                WesternBodyHtml = article.WesternBodyHtml,
                WesternCreditsHtml = article.WesternCreditsHtml,
                ExistingWesternPdfPath = article.WesternPdfPath,
                EasternTitle = article.EasternTitle,
                EasternIntroduction = article.EasternIntroduction,
                EasternBodyHtml = article.EasternBodyHtml,
                EasternCreditsHtml = article.EasternCreditsHtml,
                ExistingEasternPdfPath = article.EasternPdfPath,
                Status = article.Status,
                PublishAtUtc = article.PublishAtUtc
            };
        }

        private static void RestoreArticleFormFiles(
            MagazineArticleFormViewModel model,
            MagazineArticle article)
        {
            model.ExistingBannerImage = article.BannerImage;
            model.ExistingWesternPdfPath = article.WesternPdfPath;
            model.ExistingEasternPdfPath = article.EasternPdfPath;
        }

        private static void NormalizeArticleForm(
            MagazineArticleFormViewModel model)
        {
            model.Title = model.Title?.Trim() ?? string.Empty;

            model.ShortDescription = NormalizeOptionalString(
                model.ShortDescription);

            model.WesternTitle = NormalizeOptionalString(
                model.WesternTitle);

            model.WesternIntroduction = NormalizeOptionalString(
                model.WesternIntroduction);

            // TODO: Sanitize HTML fields before persistence when a server-side
            // HTML sanitizer is added to the project.
            model.WesternBodyHtml = NormalizeOptionalString(
                model.WesternBodyHtml);

            model.WesternCreditsHtml = NormalizeOptionalString(
                model.WesternCreditsHtml);

            model.EasternTitle = NormalizeOptionalString(
                model.EasternTitle);

            model.EasternIntroduction = NormalizeOptionalString(
                model.EasternIntroduction);

            model.EasternBodyHtml = NormalizeOptionalString(
                model.EasternBodyHtml);

            model.EasternCreditsHtml = NormalizeOptionalString(
                model.EasternCreditsHtml);
        }

        private static string? NormalizeOptionalString(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }

        private void ValidateArticleNumber(
            MagazineArticleFormViewModel model)
        {
            if (model.ArticleNumber < 1)
            {
                ModelState.AddModelError(
                    nameof(model.ArticleNumber),
                    "Article number must be at least 1.");

                return;
            }

            if (_unitOfWork.MagazineArticles.ArticleNumberExists(
                model.MagazineId,
                model.ArticleNumber,
                model.Id > 0 ? model.Id : null))
            {
                ModelState.AddModelError(
                    nameof(model.ArticleNumber),
                    "The article number is already used in this magazine.");
            }
        }

        private void ValidateArticlePublication(
            MagazineArticleFormViewModel model)
        {
            switch (model.Status)
            {
                case MagazineArticleStatus.Draft:
                case MagazineArticleStatus.Archived:
                    model.PublishAtUtc = null;
                    break;

                case MagazineArticleStatus.Published:
                    if (model.PublishAtUtc.HasValue &&
                        model.PublishAtUtc.Value.Year < 1900)
                    {
                        ModelState.AddModelError(
                            nameof(model.PublishAtUtc),
                            "Please enter a valid publication date.");
                    }
                    break;

                case MagazineArticleStatus.Scheduled:
                    if (!model.PublishAtUtc.HasValue)
                    {
                        ModelState.AddModelError(
                            nameof(model.PublishAtUtc),
                            "A publish date is required for scheduled articles.");
                    }
                    else if (model.PublishAtUtc.Value <= DateTime.UtcNow)
                    {
                        ModelState.AddModelError(
                            nameof(model.PublishAtUtc),
                            "The scheduled publication date must be in the future.");
                    }
                    break;
            }
        }

        private void ValidateArticleContent(
            MagazineArticleFormViewModel model)
        {
            if (model.Status != MagazineArticleStatus.Scheduled &&
                model.Status != MagazineArticleStatus.Published)
            {
                return;
            }

            var westernComplete = IsWesternDialectComplete(model);
            var easternComplete = IsEasternDialectComplete(model);

            if (!westernComplete && !easternComplete)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "At least one complete dialect version is required. Each complete version needs a title and article HTML.");
            }
        }

        private static bool IsWesternDialectComplete(
            MagazineArticleFormViewModel model)
        {
            return !string.IsNullOrWhiteSpace(model.WesternTitle) &&
                !string.IsNullOrWhiteSpace(model.WesternBodyHtml);
        }

        private static bool IsEasternDialectComplete(
            MagazineArticleFormViewModel model)
        {
            return !string.IsNullOrWhiteSpace(model.EasternTitle) &&
                !string.IsNullOrWhiteSpace(model.EasternBodyHtml);
        }

        private static void ApplyArticlePublication(
            MagazineArticle article,
            MagazineArticleFormViewModel model)
        {
            article.Status = model.Status;

            switch (model.Status)
            {
                case MagazineArticleStatus.Draft:
                case MagazineArticleStatus.Archived:
                    article.PublishAtUtc = null;
                    break;

                case MagazineArticleStatus.Published:
                    article.PublishAtUtc =
                        model.PublishAtUtc ?? DateTime.UtcNow;
                    break;

                case MagazineArticleStatus.Scheduled:
                    article.PublishAtUtc = model.PublishAtUtc;
                    break;
            }
        }

        private void ValidateArticleBanner(IFormFile? image)
        {
            if (image == null)
            {
                return;
            }

            if (image.Length <= 0)
            {
                ModelState.AddModelError(
                    nameof(MagazineArticleFormViewModel.BannerImageFile),
                    "The selected banner image is empty.");

                return;
            }

            if (image.Length > MaximumImageSize)
            {
                ModelState.AddModelError(
                    nameof(MagazineArticleFormViewModel.BannerImageFile),
                    "The banner image cannot exceed 5 MB.");
            }

            var extension = Path
                .GetExtension(image.FileName)
                .ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    nameof(MagazineArticleFormViewModel.BannerImageFile),
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.");
            }
        }

        private void ValidateArticlePdf(
            IFormFile? pdf,
            string propertyName,
            string displayName)
        {
            if (pdf == null)
            {
                return;
            }

            if (pdf.Length <= 0)
            {
                ModelState.AddModelError(
                    propertyName,
                    $"The selected {displayName} is empty.");

                return;
            }

            if (pdf.Length > MaximumArticlePdfSize)
            {
                ModelState.AddModelError(
                    propertyName,
                    $"The {displayName} cannot exceed 25 MB.");
            }

            var extension = Path
                .GetExtension(pdf.FileName)
                .ToLowerInvariant();

            if (!AllowedPdfExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    propertyName,
                    "Only PDF files are allowed.");

                return;
            }

            if (!string.IsNullOrWhiteSpace(pdf.ContentType) &&
                !AllowedPdfMimeTypes.Contains(
                    pdf.ContentType.ToLowerInvariant()))
            {
                ModelState.AddModelError(
                    propertyName,
                    "Only PDF files are allowed.");
            }
        }

        private async Task<string> SaveArticleImage(IFormFile image)
        {
            await _blobStorage.UploadBlobImageAsync(image);

            const string imageBasePath =
                "https://betkanublob.blob.core.windows.net/betkanublob/products/";

            var filename = Path.GetFileNameWithoutExtension(image.FileName);
            var extension = Path.GetExtension(image.FileName);

            return imageBasePath + filename + extension;
        }

        private async Task<string> SaveArticlePdf(
            IFormFile pdf,
            string source)
        {
            await _blobStorage.UploadBlobFileAsync(pdf);

            const string pdfBasePath =
                "https://betkanublob.blob.core.windows.net/betkanublob/PDF/";

            var filename = Path.GetFileNameWithoutExtension(pdf.FileName);
            var extension = Path.GetExtension(pdf.FileName);

            _ = source;

            return pdfBasePath + filename + extension;
        }

        // =====================================================
        // MAGAZINE HELPERS
        // =====================================================

        private static void NormalizeForm(
            MagazineFormViewModel model)
        {
            model.Title = model.Title?.Trim()
                ?? string.Empty;

            model.ShortDescription =
                string.IsNullOrWhiteSpace(model.ShortDescription)
                    ? null
                    : model.ShortDescription.Trim();

            model.LongDescription =
                string.IsNullOrWhiteSpace(model.LongDescription)
                    ? null
                    : model.LongDescription.Trim();
        }

        private void ValidatePublication(
            MagazineFormViewModel model)
        {
            if (!model.IsPublished)
            {
                model.PublishedAtUtc = null;
                return;
            }

            if (model.PublishedAtUtc.HasValue &&
                model.PublishedAtUtc.Value.Year < 1900)
            {
                ModelState.AddModelError(
                    nameof(model.PublishedAtUtc),
                    "Please enter a valid publication date.");
            }
        }

        private void ValidateCoverImage(IFormFile? image)
        {
            if (image == null)
            {
                return;
            }

            if (image.Length <= 0)
            {
                ModelState.AddModelError(
                    nameof(MagazineFormViewModel.CoverImageFile),
                    "The selected image is empty.");

                return;
            }

            if (image.Length > MaximumImageSize)
            {
                ModelState.AddModelError(
                    nameof(MagazineFormViewModel.CoverImageFile),
                    "The cover image cannot exceed 5 MB.");
            }

            var extension = Path
                .GetExtension(image.FileName)
                .ToLowerInvariant();

            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError(
                    nameof(MagazineFormViewModel.CoverImageFile),
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.");
            }
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            string Pathimg = @"https://betkanublob.blob.core.windows.net/betkanublob/products/";
            string filename = Path.GetFileNameWithoutExtension(image.FileName);
            string ex = Path.GetExtension(image.FileName);
            string ImgName = Pathimg + filename + ex;
            await _blobStorage.UploadBlobImageAsync(image);
            return ImgName;
        }
    }
}
