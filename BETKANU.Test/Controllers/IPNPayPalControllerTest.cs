

using BET_KANU.Controllers;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BetKanu.Test.Controllers
{
    public class IPNPayPalControllerTest
    {
        [Fact]
        public void Test_IPN_Notification_Controller()
        {
            // Arrange
            PayPalIPNController controller = new();

            var mockIPNData = "mock_ipn_data";

            var mockRequest = new Mock<HttpRequest>();
            var mockRequestBodyStream = new Mock<Stream>();

            // Mock stream reader to return IPN data
            var mockRequestBodyReader = new Mock<StreamReader>();
            mockRequestBodyReader.Setup(x => x.ReadToEnd()).Returns(mockIPNData);

            // Setup mock body stream to return stream reader
            // Create mock byte array
            var mockBytes = new byte[1024];

            // Setup mock stream to return data when Read is called
            mockRequestBodyStream.Setup(x => x.Read(It.IsAny<byte[]>()))
                                .Callback<byte[]>(bytes => Array.Copy(mockBytes, bytes, mockBytes.Length))
                                .Returns(mockBytes.Length);

            // Mock HttpContext
            var mockHttpContext = new Mock<HttpContext>();

            // Set up Request to return mock request 
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

            // Create controller context
            var controllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Set on controller 
            controller.ControllerContext = controllerContext;

            //var mockVerifier = new Mock<IPayPalIPNVerifier>();
            //mockVerifier.Setup(x => x.Verify(mockIPNData))
            //            .Returns(true);

            // Act
            var result = controller.Receive();

            // Assert 
            Assert.IsType<RedirectToRouteResult>(result);
            var redirectResult = result as RedirectToRouteResult;
            Assert.Equal("SpecialThanks", redirectResult.RouteValues["action"]);
            Assert.Equal("Home", redirectResult.RouteValues["controller"]);
        }
    }
}
