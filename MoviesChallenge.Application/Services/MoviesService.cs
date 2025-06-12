using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesChallenge.Infrastructure.Clients;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;
using System.Net.Http;

namespace MoviesChallenge.Application.Services
{
    public class MoviesService : IMovieService
    {
        private readonly IMovieApiClient _apiClient;

        public MoviesService(IMovieApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<MoviesResponse>> GetAllAsync()
        {
            var data = await _apiClient.FetchMoviesAsync();

            var result = data?
                .Where(m => m.Latitude != 0 && m.Longitude != 0)
                .Select(m => new MoviesResponse
                {
                    Title = m.Title,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude,
                    DistanceKm = 0,
                    Release_Year = m.Release_Year,
                    Locations = m.Locations,
                    Production_Company = m.Production_Company
                })
                .ToList();

            return result ?? new List<MoviesResponse>();
        }

        public async Task<IEnumerable<MoviesResponse>> GetNearAsync(MoviesRequest request)
        {
            var movies = await _apiClient.FetchMoviesAsync();

            var nearby = movies
                .Select(m => {
                    var distance = GeoUtils.CalculateDistanceKm(request.Lat, request.Lng, m.Latitude, m.Longitude);
                    return new MoviesResponse
                    {
                        Title = m.Title,
                        Release_Year = m.Release_Year,
                        Locations = m.Locations,
                        Production_Company = m.Production_Company,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        DistanceKm = distance
                    };
                })
                .Where(m => m.DistanceKm <= request.Radius)
                .OrderBy(m => m.DistanceKm)
                .ToList();

            return nearby;
        }

        public static class GeoUtils
        {
            //Calcula las distancias en linea recta en base a longitud y latitud. 
            public static double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
            {
                const double R = 6371;
                var dLat = ToRadians(lat2 - lat1);
                var dLon = ToRadians(lon2 - lon1);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return R * c;
            }

            //Convierte a radianes las longitudes y latitudes, para poder usar las funciones trigonometricas Math.Cos y Math.Sin
            private static double ToRadians(double angle) => angle * Math.PI / 180;
        }
    }
}
