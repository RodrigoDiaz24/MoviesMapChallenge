using MoviesChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesChallenge.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MoviesResponse>> GetAllAsync();
        Task<IEnumerable<MoviesResponse>> GetNearAsync(MoviesRequest request);
    }
}
