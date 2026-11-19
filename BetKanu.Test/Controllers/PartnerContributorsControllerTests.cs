using BET_KANU.Controllers;
using BET_KANU.Services;
using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BetKanu.Test.Controllers;

public class PartnerContributorsControllerTests
{
    private readonly Mock<IPartnerContributorService> _partners = new();

    [Fact]
    public async Task Index_returns_view_with_paged_results()
    {
        _partners.Setup(x => x.GetAdminPagedAsync(It.IsAny<PartnerContributorFilterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<PartnerContributorListDto>
            {
                Items = new List<PartnerContributorListDto>
                {
                    new() { Id = 1, Name = "Test Org", Category = CollaborationCategory.Partner, IsActive = true }
                },
                Page = 1,
                PageSize = 20,
                TotalCount = 1
            });

        var controller = new PartnerContributorsController(_partners.Object);
        var result = await controller.Index(null, null, null, null);

        var view = Assert.IsType<ViewResult>(result);
        Assert.NotNull(view.Model);
    }

    [Fact]
    public async Task Edit_get_returns_not_found_for_missing_org()
    {
        _partners.Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PartnerContributorDetailsDto?)null);

        var controller = new PartnerContributorsController(_partners.Object);
        var result = await controller.Edit(99);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_uses_service_and_redirects()
    {
        _partners.Setup(x => x.DeleteAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ServiceResult.Failure("Cannot delete while projects exist."));

        var controller = new PartnerContributorsController(_partners.Object);
        controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
            new Microsoft.AspNetCore.Http.DefaultHttpContext(),
            Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

        var result = await controller.Delete(5, null, CancellationToken.None);

        Assert.IsType<RedirectToActionResult>(result);
        _partners.Verify(x => x.DeleteAsync(5, It.IsAny<CancellationToken>()), Times.Once);
    }
}
