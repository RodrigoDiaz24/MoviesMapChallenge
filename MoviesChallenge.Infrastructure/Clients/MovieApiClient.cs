using MoviesChallenge.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MoviesChallenge.Infrastructure.Clients
{
    public class MovieApiClient : IMovieApiClient
    {
        private readonly HttpClient _httpClient;

        public MovieApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExternalMovieDto>> FetchMoviesAsync()
        {
            try
            {
                var movies = await _httpClient.GetFromJsonAsync<IEnumerable<ExternalMovieDto>>("https://data.sfgov.org/resource/yitu-d5am");

                return movies ?? new List<ExternalMovieDto>();
            }
            catch (Exception ex)
            {
                return new List<ExternalMovieDto>();
            }
        }
    }
}
