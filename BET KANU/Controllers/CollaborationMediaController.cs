using BET_KANU.Services;
using BetKanu.Models.DTOs.Collaboration;
using BetKanu.Models.ViewModels.Collaboration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers;

[Authorize]
[Route("admin/collaboration-media")]
public sealed class CollaborationMediaController : Controller
{
    private readonly ICollaborationMediaService _media;
    private readonly IPartnerContributorService _partners;
    private readonly ICollaborationProjectService _projects;

    public CollaborationMediaController(
        ICollaborationMediaService media,
        IPartnerContributorService partners,
        ICollaborationProjectService projects)
    {
        _media = media;
        _partners = partners;
        _projects = projects;
    }

    [HttpPost("upload")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(120_000_000)]
    public async Task<IActionResult> Upload(CollaborationMediaUploadFormViewModel form, CancellationToken ct)
    {
        var parentCheck = await ValidateParentOwnershipAsync(form.PartnerContributorId, form.CollaborationProjectId, ct);
        if (parentCheck != null)
        {
            TempData["CollabError"] = parentCheck;
            return RedirectBack(form.ReturnUrl, form.PartnerContributorId, form.CollaborationProjectId);
        }

        if (form.Files == null || form.Files.Count == 0)
        {
            TempData["CollabError"] = "Select at least one file to upload.";
            return RedirectBack(form.ReturnUrl, form.PartnerContributorId, form.CollaborationProjectId);
        }

        var dto = new CollaborationMediaUploadDto
        {
            PartnerContributorId = form.PartnerContributorId,
            CollaborationProjectId = form.CollaborationProjectId,
            Title = form.Title,
            Caption = form.Caption,
            Description = form.Description,
            DisplayOrder = form.DisplayOrder,
            IsActive = form.IsActive
        };

        var result = await _media.UploadManyAsync(dto, form.Files, ct);
        if (!result.Succeeded)
        {
            TempData["CollabError"] = result.Error ?? "Upload failed.";
        }
        else
        {
            var uploaded = result.Data?.Count ?? 0;
            TempData["CollabSuccess"] = uploaded == 1
                ? "1 file uploaded successfully."
                : $"{uploaded} files uploaded successfully.";
            if (result.Errors.Count > 0)
                TempData["CollabWarning"] = string.Join(" ", result.Errors);
        }

        return RedirectBack(form.ReturnUrl, form.PartnerContributorId, form.CollaborationProjectId);
    }

    [HttpPost("update/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, CollaborationMediaEditViewModel form, CancellationToken ct)
    {
        if (form.Id != id) return BadRequest();

        var existing = await _media.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var result = await _media.UpdateAsync(id, form.ToUpdateDto(), ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded ? "Media details updated." : result.Error;

        return RedirectBack(form.ReturnUrl, existing.PartnerContributorId, existing.CollaborationProjectId);
    }

    [HttpPost("set-active/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActive(int id, bool isActive, string? returnUrl, CancellationToken ct)
    {
        var existing = await _media.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var result = await _media.SetActiveAsync(id, isActive, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded
                ? (isActive ? "Media activated." : "Media deactivated.")
                : result.Error;

        return RedirectBack(returnUrl, existing.PartnerContributorId, existing.CollaborationProjectId);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl, CancellationToken ct)
    {
        var existing = await _media.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        var result = await _media.DeleteAsync(id, ct);
        TempData[result.Succeeded ? "CollabSuccess" : "CollabError"] =
            result.Succeeded ? "Media deleted." : result.Error;

        return RedirectBack(returnUrl, existing.PartnerContributorId, existing.CollaborationProjectId);
    }

    private async Task<string?> ValidateParentOwnershipAsync(
        int? partnerId,
        int? projectId,
        CancellationToken ct)
    {
        var hasPartner = partnerId.HasValue && partnerId.Value > 0;
        var hasProject = projectId.HasValue && projectId.Value > 0;

        if (hasPartner == hasProject)
            return "Media must belong to exactly one parent organization or project.";

        if (hasPartner)
        {
            var partner = await _partners.GetByIdAsync(partnerId!.Value, ct);
            return partner is null ? "Organization was not found." : null;
        }

        var project = await _projects.GetByIdAsync(projectId!.Value, ct);
        return project is null ? "Project was not found." : null;
    }

    private IActionResult RedirectBack(string? returnUrl, int? partnerId, int? projectId)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        if (projectId.HasValue)
            return RedirectToAction("Edit", "CollaborationProjects", new { id = projectId.Value, tab = "media" });

        if (partnerId.HasValue)
            return RedirectToAction("Edit", "PartnerContributors", new { id = partnerId.Value, tab = "media" });

        return RedirectToAction("Index", "PartnerContributors");
    }
}
