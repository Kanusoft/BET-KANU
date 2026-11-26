using BET_KANU.Services;
using BetKanu.Models.Common;
using BetKanu.Models.Interface;
using BetKanu.Models.ViewModels.Collaboration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BET_KANU.Controllers;

[Authorize]
[Route("admin/collaboration-projects")]
public sealed class CollaborationProjectsController : Controller
{
    private readonly ICollaborationProjectService _projects;
    private readonly IPartnerContributorService _partners;
    private readonly IUnitOfWork _unitOfWork;

    public CollaborationProjectsController(
        ICollaborationProjectService projects,
        IPartnerContributorService partners,
        IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _partners = partners;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(int partnerId, CancellationToken ct)
    {
        var partner = await _partners.GetByIdAsync(partnerId, ct);
        if (partner is null) return NotFound();

        return View("Edit", new CollaborationProjectFormViewModel
        {
            PartnerContributorId = partner.Id,
            PartnerContributorName = partner.Name,
            ContentOptions = BuildContentOptions()
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CollaborationProjectFormViewModel form, CancellationToken ct)
    {
        form.ContentOptions = BuildContentOptions();
        ValidateForm(form);
        if (!ModelState.IsValid)
            return View("Edit", form);

        if (!form.TryApplyContentSelection(out var contentType, out var contentId, out var contentError))
        {
            ModelState.AddModelError(nameof(form.ContentSelection), contentError ?? "The selected content is invalid.");
            return View("Edit", form);
        }

        var partner = await _partners.GetByIdAsync(form.PartnerContributorId, ct);
        if (partner is null)
        {
            ModelState.AddModelError(nameof(form.PartnerContributorId), "The selected organization was not found.");
            return View("Edit", form);
        }

        form.PartnerContributorName = partner.Name;
        var result = await _projects.CreateAsync(form.ToCreateDto(contentType, contentId), ct);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return View("Edit", form);
        }

        if (form.LogoFile != null)
        {
            var logo = await _projects.UploadLogoAsync(result.Data!.Id, form.LogoFile, ct);
            if (!logo.Succeeded)
                TempData["CollabWarning"] = logo.Error ?? "Project saved, but logo upload failed.";
        }

        TempData["CollabSuccess"] = "Project created successfully.";
        return RedirectToAction(nameof(Edit), new { id = result.Data!.Id });
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(
        int id,
        string tab = "information",
        CollaborationMediaType? mediaType = null,
        CancellationToken ct = default)
    {
        var entity = await _projects.GetByIdAsync(id, ct);
        if (entity is null) return NotFound();

        var model = CollaborationProjectFormViewModel.FromDetails(entity, BuildContentOptions());
        model.ActiveTab = tab?.Equals("media", StringComparison.OrdinalIgnoreCase) == true
            ? "media"
            : "information";
        ViewBag.MediaType = mediaType;
        return View(model);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CollaborationProjectFormViewModel form, CancellationToken ct)
    {
        if (form.Id != id) return BadRequest();

        form.ContentOptions = BuildContentOptions();
        ValidateForm(form);
        if (!ModelState.IsValid)
        {
            await ReloadMediaAsync(form, ct);
            return View(form);
        }

        if (!form.TryApplyContentSelection(out var contentType, out var contentId, out var contentError))
        {
            ModelState.AddModelError(nameof(form.ContentSelection), contentError ?? "The selected content is invalid.");
            await ReloadMediaAsync(form, ct);
            return View(form);
        }

        var partner = await _partners.GetByIdAsync(form.PartnerContributorId, ct);
        if (partner is null)
        {
            ModelState.AddModelError(nameof(form.PartnerContributorId), "The selected organization was not found.");
            await ReloadMediaAsync(form, ct);
            return View(form);
        }

        var result = await _projects.UpdateAsync(id, form.ToUpdateDto(contentType, contentId), ct);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            await ReloadMediaAsync(form, ct);
            return View(form);
        }

        if (form.LogoFile != null)
        {
            var logo = await _projects.UploadLogoAsync(id, form.LogoFile, ct);
            if (!logo.Succeeded)
                TempData["CollabWarning"] = logo.Error ?? "Details saved, but logo upload failed.";
        }

        TempData["CollabSuccess"] = "Project updated successfully.";
        return RedirectToAction(nameof(Edit), new { id, tab = "information" });
    }

    [HttpPost("set-active/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActive(int id, bool isActive, string? returnUrl, CancellationToken ct)
    {
        var result = await _projects.SetActiveAsync(id, isActive, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded
                ? (isActive ? "Project activated." : "Project deactivated.")
                : result.Error;

        return LocalRedirectOrPartner(returnUrl, id, ct);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl, CancellationToken ct)
    {
        var existing = await _projects.GetByIdAsync(id, ct);
        var partnerId = existing?.PartnerContributorId;

        var result = await _projects.DeleteAsync(id, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded
                ? "Project deleted."
                : result.Error ?? "Unable to delete this project.";

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        if (partnerId.HasValue)
            return RedirectToAction("Edit", "PartnerContributors", new { id = partnerId.Value, tab = "projects" });

        return RedirectToAction("Index", "PartnerContributors");
    }

    private async Task ReloadMediaAsync(CollaborationProjectFormViewModel form, CancellationToken ct)
    {
        if (!form.Id.HasValue) return;
        var entity = await _projects.GetByIdAsync(form.Id.Value, ct);
        if (entity is null) return;
        form.ExistingLogoUrl = entity.LogoUrl;
        form.Media = entity.Media;
        form.PartnerContributorName = entity.PartnerContributorName;
    }

    private void ValidateForm(CollaborationProjectFormViewModel form)
    {
        if (string.IsNullOrWhiteSpace(form.Title))
            ModelState.AddModelError(nameof(form.Title), "Project title is required.");

        if (form.PartnerContributorId <= 0)
            ModelState.AddModelError(nameof(form.PartnerContributorId), "Organization is required.");

        if (!string.IsNullOrWhiteSpace(form.ProjectLink))
        {
            if (!Uri.TryCreate(form.ProjectLink, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(nameof(form.ProjectLink), "Project link must start with http:// or https://.");
            }
        }

        if (form.ContentPageId.HasValue && form.ContentPageId.Value <= 0)
            ModelState.AddModelError(nameof(form.ContentPageId), "Content page ID must be a positive number when provided.");
    }

    private IEnumerable<CollaborationContentOptionItem> BuildContentOptions()
    {
        var options = new List<CollaborationContentOptionItem>
        {
            new() { Value = "", Text = "(None)", Group = "" }
        };

        var books = (_unitOfWork.bKBundle.GetBooks() ?? Enumerable.Empty<BetKanu.Models.Book>())
            .Select(b => new CollaborationContentOptionItem
            {
                Value = CollaborationContentSelection.Format(CollaborationContentType.ReaderBook, b.Id),
                Text = string.IsNullOrWhiteSpace(b.Name) ? $"Book #{b.Id}" : b.Name!,
                Group = CollaborationContentSelection.GroupLabel(CollaborationContentType.ReaderBook)
            })
            .OrderBy(x => x.Text, StringComparer.OrdinalIgnoreCase)
            .ToList();

        options.AddRange(books);

        var products = (_unitOfWork.product.GetAll() ?? Enumerable.Empty<BetKanu.Models.Product>())
            .OrderBy(p => (int)p.Category)
            .ThenBy(p => p.Title, StringComparer.OrdinalIgnoreCase)
            .Select(p => new CollaborationContentOptionItem
            {
                Value = CollaborationContentSelection.Format(CollaborationContentType.Product, p.Id),
                Text = string.IsNullOrWhiteSpace(p.Title) ? $"Product #{p.Id}" : p.Title!,
                Group = CollaborationContentSelection.GroupLabel(CollaborationContentType.Product, p.Category)
            })
            .ToList();

        options.AddRange(products);
        return options;
    }

    private IActionResult LocalRedirectOrPartner(string? returnUrl, int projectId, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction(nameof(Edit), new { id = projectId });
    }
}
