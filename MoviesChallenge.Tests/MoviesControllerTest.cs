using Xunit;
using Moq;
using MoviesChallenge.API.Controllers;
using MoviesChallenge.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Tests
{
    public class MoviesControllerTest
    {
        [Fact]
        public async Task GetNearby_ReturnsOkResult_WithExpectedMovies()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();
            var response = new MoviesResponse
            {
                Title = "El Eternauta",
                Latitude = -34.60,
                Longitude = -58.38,
                DistanceKm = 1.2
            }; 

            mockService.Setup(s =>
                s.GetNearAsync(It.IsAny<MoviesRequest>())
              ).ReturnsAsync(new List<MoviesResponse> { response });

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetNearby(It.IsAny<MoviesRequest>());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<MoviesResponse>>(okResult.Value);

            Assert.Single(data);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithMoviesList()
        {
            // Arrange
            var mockService = new Mock<IMovieService>();

            mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<MoviesResponse>
                {
                new MoviesResponse
                {
                    Title = "El Eternauta",
                    Latitude = 37.8267,
                    Longitude = -122.423,
                    DistanceKm = 0,
                    Release_Year = 1996,
                    Locations = "Buenos Aires",
                    Production_Company = "Canal 13"
                }
                });

            var controller = new MoviesController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<MoviesResponse>>(okResult.Value);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoMoviesFound()
        {
            var mockService = new Mock<IMovieService>();
            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<MoviesResponse>());

            var controller = new MoviesController(mockService.Object);

            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<MoviesResponse>>(okResult.Value);
            Assert.Empty(data);
        }
    }
}
