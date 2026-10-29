
using BET_KANU.Services;
using BetKanu.Models.Common;
using BetKanu.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BET_KANU.Controllers;

[Authorize]
[Route("admin/supporters")]
public sealed class AdminSupportersController : Controller
{
    private readonly ISupporterService _supporters;
    public AdminSupportersController(ISupporterService supporters) => _supporters = supporters;

    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? q,
        SupporterType? type,
        int? year,
        bool? published,
        int? period,
        CancellationToken ct)
    {
        var rows = await _supporters.SearchAdminAsync(q, type, year, published, ct, period);
        return View(new SupporterAdminIndexViewModel
        {
            Query = q,
            Type = type,
            Year = year,
            Published = published,
            Period = period,
            Items = rows
        });
    }

    [HttpGet("create")]
    public IActionResult Create() => View("Edit", new SupporterFormViewModel());

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupporterFormViewModel form, CancellationToken ct)
    {
        ValidateForm(form, isCreate: true);
        if (!ModelState.IsValid) return View("Edit", form);

        await _supporters.CreateAsync(form.ToEntity(), ct);
        TempData["SupporterSuccess"] = "Supporter created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var entity = await _supporters.GetAsync(id, ct);
        return entity is null ? NotFound() : View(SupporterFormViewModel.FromEntity(entity));
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SupporterFormViewModel form, CancellationToken ct)
    {
        if (form.Id != id) return BadRequest();
        ValidateForm(form, isCreate: false);
        if (!ModelState.IsValid) return View(form);

        if (!await _supporters.UpdateAsync(id, form.ToEntity(), ct)) return NotFound();
        TempData["SupporterSuccess"] = "Supporter updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("publish/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPublished(int id, bool published, CancellationToken ct)
    {
        if (!await _supporters.SetPublishedAsync(id, published, ct)) return NotFound();
        TempData["SupporterSuccess"] = published ? "Supporter published." : "Supporter hidden.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (!await _supporters.DeleteAsync(id, ct)) return NotFound();
        TempData["SupporterSuccess"] = "Supporter deleted.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidateForm(SupporterFormViewModel form, bool isCreate)
    {
        if (string.IsNullOrWhiteSpace(form.Name))
            ModelState.AddModelError(nameof(form.Name), "Supporter name cannot be empty.");

        if (form.Type is null || !Enum.IsDefined(typeof(SupporterType), form.Type.Value))
            ModelState.AddModelError(nameof(form.Type), "Select a valid supporter type.");

        if (form.Period is null || !Enum.IsDefined(typeof(SupporterPeriod), form.Period.Value))
            ModelState.AddModelError(nameof(form.Period), "Select a valid period.");

        if (form.UnknownHistoricalYear)
        {
            // Preserve legacy "year unknown" as Year = 0; do not allow on brand-new creates.
            if (isCreate)
            {
                ModelState.AddModelError(
                    nameof(form.RecognitionYear),
                    "New supporters require a recognition year. Use edit only for legacy unknown-year records.");
            }
            else
            {
                form.RecognitionYear = null;
                ModelState.Remove(nameof(form.RecognitionYear));
            }
        }
        else if (!form.RecognitionYear.HasValue)
        {
            ModelState.AddModelError(nameof(form.RecognitionYear), "Enter a recognition year.");
        }
        else if (form.RecognitionYear.Value < 1900 || form.RecognitionYear.Value > 2100)
        {
            ModelState.AddModelError(nameof(form.RecognitionYear), "Enter a valid recognition year between 1900 and 2100.");
        }
    }
}
