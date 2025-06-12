using Xunit;
using Moq;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Infrastructure.Clients;
using MoviesChallenge.Infrastructure.Models;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;

public class MovieServiceTests
{
    [Fact]
    public async Task GetNearbyAsync_ReturnsOnlyMoviesWithinRadius()
    {
        // Arrange
        var mockApiClient = new Mock<IMovieApiClient>();

        mockApiClient.Setup(client => client.FetchMoviesAsync())
            .ReturnsAsync(new List<ExternalMovieDto>
            {
                new ExternalMovieDto { Title = "El Eternauta", Latitude = -34.6037, Longitude = -58.3816 }, // Cerca
                new ExternalMovieDto { Title = "Son Amores", Latitude = -34.7000, Longitude = -58.5000 }, // Lejos
            });

        var service = new MoviesService(mockApiClient.Object);

        var request = new MoviesRequest
        {
            Lat = -34.6037,
            Lng = -58.3816,
            Radius = 5.0
        };

        // Act
        var result = await service.GetNearAsync(request);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, m => m.Title == "El Eternauta");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedMovies_WithValidCoordinates()
    {
        // Arrange
        var mockApiClient = new Mock<IMovieApiClient>();

        mockApiClient.Setup(x => x.FetchMoviesAsync())
            .ReturnsAsync(new List<ExternalMovieDto>
            {
                new ExternalMovieDto
                {
                    Title = "El Eternauta",
                    Latitude = 37.8267,
                    Longitude = -122.423,
                    Release_Year = 1996,
                    Locations = "Buenos Aires",
                    Production_Company = "Canal 13"
                },
                new ExternalMovieDto
                {
                    Title = "No Location",
                    Latitude = 0,
                    Longitude = 0
                }
            });

        var service = new MoviesService(mockApiClient.Object);

        // Act
        var result = (await service.GetAllAsync()).ToList();

        // Assert
        Assert.Single(result); 
        Assert.Equal("El Eternauta", result[0].Title);
        Assert.Equal("Buenos Aires", result[0].Locations);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenApiReturnsNull()
    {
        var mockApiClient = new Mock<IMovieApiClient>();
        mockApiClient.Setup(x => x.FetchMoviesAsync()).ReturnsAsync((List<ExternalMovieDto>)null!);

        var service = new MoviesService(mockApiClient.Object);

        var result = await service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}