using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieoBackendLA0110.Model
{
    public class Movie
    {
        public int MovieID { get; set; }
        public required string MovieTitle { get; set; }
    }
}
