using BET_KANU.Services;
using BetKanu.Models;
using BetKanu.Models.Interface;
using BetKanu.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace BET_KANU.Controllers
{
    [Authorize]
    public class MagazineManagerController : Controller
    {
        private const long MaximumImageSize = 5 * 1024 * 1024;

        private static readonly string[] AllowedImageExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
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
        // HELPERS
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

        private static string GenerateSlug(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var slug = value.Trim().ToLowerInvariant();

            slug = Regex.Replace(
                slug,
                @"[^a-z0-9\s-]",
                string.Empty);

            slug = Regex.Replace(
                slug,
                @"\s+",
                "-");

            slug = Regex.Replace(
                slug,
                @"-+",
                "-");

            return slug.Trim('-');
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
