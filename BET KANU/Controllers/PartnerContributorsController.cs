using BET_KANU.Services;
using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using BetKanu.Models.ViewModels.Collaboration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CollaborationMediaType = BetKanu.Models.Common.CollaborationMediaType;

namespace BET_KANU.Controllers;

[Authorize]
[Route("admin/partners-contributors")]
public sealed class PartnerContributorsController : Controller
{
    private readonly IPartnerContributorService _partners;

    public PartnerContributorsController(IPartnerContributorService partners)
    {
        _partners = partners;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? search,
        CollaborationCategory? category,
        bool? isActive,
        int? year,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var results = await _partners.GetAdminPagedAsync(new PartnerContributorFilterDto
        {
            Search = search,
            Category = category,
            IsActive = isActive,
            Year = year,
            Page = page,
            PageSize = pageSize
        }, ct);

        return View(new PartnerContributorIndexViewModel
        {
            Search = search,
            Category = category,
            IsActive = isActive,
            Year = year,
            Page = results.Page,
            PageSize = results.PageSize,
            Results = results
        });
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Edit", new PartnerContributorFormViewModel());

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PartnerContributorFormViewModel form, CancellationToken ct)
    {
        ValidateForm(form);
        if (!ModelState.IsValid)
            return View("Edit", form);

        var result = await _partners.CreateAsync(form.ToCreateDto(), ct);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return View("Edit", form);
        }

        if (form.LogoFile != null)
        {
            var logo = await _partners.UploadLogoAsync(result.Data!.Id, form.LogoFile, ct);
            if (!logo.Succeeded)
                TempData["CollabWarning"] = logo.Error ?? "Organization saved, but logo upload failed.";
        }

        TempData["CollabSuccess"] = "Organization created successfully.";
        return RedirectToAction(nameof(Edit), new { id = result.Data!.Id });
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(
        int id,
        string tab = "information",
        CollaborationMediaType? mediaType = null,
        CancellationToken ct = default)
    {
        var entity = await _partners.GetByIdAsync(id, ct);
        if (entity is null) return NotFound();

        var model = PartnerContributorFormViewModel.FromDetails(entity);
        model.ActiveTab = NormalizeTab(tab);
        ViewBag.MediaType = mediaType;
        return View(model);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PartnerContributorFormViewModel form, CancellationToken ct)
    {
        if (form.Id != id) return BadRequest();

        ValidateForm(form);
        if (!ModelState.IsValid)
        {
            await ReloadEditExtrasAsync(form, ct);
            return View(form);
        }

        var result = await _partners.UpdateAsync(id, form.ToUpdateDto(), ct);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            await ReloadEditExtrasAsync(form, ct);
            return View(form);
        }

        if (form.LogoFile != null)
        {
            var logo = await _partners.UploadLogoAsync(id, form.LogoFile, ct);
            if (!logo.Succeeded)
                TempData["CollabWarning"] = logo.Error ?? "Details saved, but logo upload failed.";
        }

        TempData["CollabSuccess"] = "Organization updated successfully.";
        return RedirectToAction(nameof(Edit), new { id, tab = "information" });
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var entity = await _partners.GetByIdAsync(id, ct);
        return entity is null
            ? NotFound()
            : View(new PartnerContributorDetailsViewModel { Item = entity });
    }

    [HttpPost("set-active/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActive(int id, bool isActive, string? returnUrl, CancellationToken ct)
    {
        var result = await _partners.SetActiveAsync(id, isActive, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded
                ? (isActive ? "Organization activated." : "Organization deactivated.")
                : result.Error;

        return LocalRedirectOrIndex(returnUrl);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl, CancellationToken ct)
    {
        var result = await _partners.DeleteAsync(id, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded
                ? "Organization deleted."
                : result.Error ?? "Unable to delete this organization.";

        return LocalRedirectOrIndex(returnUrl);
    }

    private async Task ReloadEditExtrasAsync(PartnerContributorFormViewModel form, CancellationToken ct)
    {
        if (!form.Id.HasValue) return;
        var entity = await _partners.GetByIdAsync(form.Id.Value, ct);
        if (entity is null) return;
        form.ExistingLogoUrl = entity.LogoUrl;
        form.Projects = entity.Projects;
        form.Media = entity.Media;
    }

    private void ValidateForm(PartnerContributorFormViewModel form)
    {
        if (string.IsNullOrWhiteSpace(form.Name))
            ModelState.AddModelError(nameof(form.Name), "Organization name is required.");

        if (!Enum.IsDefined(typeof(CollaborationCategory), form.Category))
            ModelState.AddModelError(nameof(form.Category), "Select a valid category.");

        if (!string.IsNullOrWhiteSpace(form.WebsiteUrl))
        {
            if (!Uri.TryCreate(form.WebsiteUrl, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError(nameof(form.WebsiteUrl), "Website URL must start with http:// or https://.");
            }
        }

        if (form.Year.HasValue && (form.Year < 1900 || form.Year > 2100))
            ModelState.AddModelError(nameof(form.Year), "Year must be between 1900 and 2100.");
    }

    private static string NormalizeTab(string? tab) =>
        tab?.Trim().ToLowerInvariant() switch
        {
            "projects" => "projects",
            "media" => "media",
            _ => "information"
        };

    private IActionResult LocalRedirectOrIndex(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction(nameof(Index));
    }
}
