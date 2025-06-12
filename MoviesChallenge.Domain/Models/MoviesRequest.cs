using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesChallenge.Domain.Models
{
    public class MoviesRequest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Radius { get; set; } = 5;
    }
}
