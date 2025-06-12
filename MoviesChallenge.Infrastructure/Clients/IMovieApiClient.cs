using MoviesChallenge.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesChallenge.Infrastructure.Clients
{
    public interface IMovieApiClient
    {
        Task<IEnumerable<ExternalMovieDto>> FetchMoviesAsync();
    }
}
