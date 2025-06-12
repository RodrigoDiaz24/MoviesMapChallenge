using Moq.Protected;
using Moq;
using MoviesChallenge.Infrastructure.Clients;
using MoviesChallenge.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MoviesChallenge.Tests
{
    public class MovieApiClientTest
    {
        [Fact]
        public async Task FetchMoviesAsync_ReturnsData_WhenApiResponds()
        {
            // Arrange
            var mockData = new List<ExternalMovieDto>
        {
            new ExternalMovieDto { Title = "Hijitus", Latitude = -34.60, Longitude = -58.38 }
        };

            var json = JsonSerializer.Serialize(mockData);

            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new System.Uri("https://fakeapi.com")
            };

            var client = new MovieApiClient(httpClient);

            // Act
            var result = await client.FetchMoviesAsync();
            var resultList = result.ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Hijitus", resultList[0].Title);
            Assert.Equal(-34.60, resultList[0].Latitude);
            Assert.Equal(-58.38, resultList[0].Longitude);
        }

        [Fact]
        public async Task FetchMoviesAsync_ReturnsEmptyList_WhenApiReturnsNull()
        {
            // Arrange: simulamos una respuesta con contenido nulo
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null") // ← importante: string `"null"`
                });

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var client = new MovieApiClient(httpClient);

            // Act
            var result = await client.FetchMoviesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
