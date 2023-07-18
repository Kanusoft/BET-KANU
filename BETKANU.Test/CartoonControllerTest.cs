

using BET_KANU.Controllers;
using BET_KANU.Services;
using BetKanu.Data;
using BetKanu.Data.Repositories;
using BetKanu.Models;
using BetKanu.Models.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BETKANU.Test
{
    public class CartoonControllerTest
    {
        private readonly MangerRepo _controller;
        private readonly Mock<BKdbContext> _mangerMock;

        public CartoonControllerTest()
        {
            // Set up dependencies and controller instance for each test
            _mangerMock = new Mock<BKdbContext>();
            _controller = new MangerRepo(_mangerMock.Object);
        }

        [Fact]
        public async Task<int> CreateEpisode_ValidData_ReturnsRedirectToActionResult()
        {
            // Arrange
            var cartoonepisode = new ProductEpisode(); // Create an instance with valid data
            int parentId = 1; // Set a value for the parentId parameter

            // Set up the mock behavior for the manager
            _mangerMock.Setup(m => m.ProductEpisodes.Add(It.IsAny<ProductEpisode>())).Returns(1); // Assuming AddEpisode returns 1 for success

            // Act
            var result =  _controller.AddEpisode(cartoonepisode) as 1;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Manger", result.ControllerName);
         
        }

        // Other test methods can be written here
    }
}


  

