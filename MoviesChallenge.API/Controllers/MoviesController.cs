using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;


namespace MoviesChallenge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _movieService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby([FromQuery] MoviesRequest request)
        {
            var results = await _movieService.GetNearAsync(request);
            return Ok(results);
        }
    }
}
