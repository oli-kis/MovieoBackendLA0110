using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieoBackendLA0110.Model;
using System.Text.RegularExpressions;

namespace MovieoBackendLA0110.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private static List<Movie> _movies = new(Convert("./Data/MovieDB.csv"));

        [HttpGet("AllMovies")]
        public Task<ActionResult<IEnumerable<Movie>>> Index()
        {
            return Task.FromResult<ActionResult<IEnumerable<Movie>>>(Ok(_movies));
        }

        [HttpGet("GetMovie/{id}")]
        public Task<ActionResult<Movie>> GetMovie(int id)
        {
            try
            {
                var movie = _movies[id - 1];
                if (movie == null)
                {
                    return Task.FromResult<ActionResult<Movie>>(NotFound());
                }

                return Task.FromResult<ActionResult<Movie>>(Ok(movie));
            }
            catch (Exception)
            {
                return Task.FromResult<ActionResult<Movie>>(NotFound("Movie is not in List"));
            }
        }

        [HttpDelete("DeleteMovie/{id}")]
        public Task<ActionResult> DeleteMovie(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return Task.FromResult<ActionResult>(NotFound());
            }
            else
            {
                _movies.Remove(movie);
                Console.WriteLine($"Movie with ID {id} has been deleted.");
            }

            return Task.FromResult<ActionResult>(NoContent());
        }

        [HttpPut("UpdateMovie")]
        public Task<ActionResult> UpdateMovie(int id, string movieName)
        {
            var movie = _movies.FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return Task.FromResult<ActionResult>(NotFound());
            }
            else
            {
                movie.MovieTitle = movieName;
                Console.WriteLine($"Movie with ID {id} has been updated.");
            }

            return Task.FromResult<ActionResult>(Ok(movie));
        }

        [HttpPost("AddMovie")]
        public Task<ActionResult> AddMovie(string movieName)
        {
            var movie = new Movie() { MovieID = _movies.Count() + 1, MovieTitle = movieName };
            _movies.Add(movie);

            return Task.FromResult<ActionResult>(Ok(movie));
        }

        private static List<Movie> Convert(string csvFilePath)
        {
            var csvText = System.IO.File.ReadAllText(csvFilePath);

            var lines = csvText.Split("\n");
            var movies = new List<Movie>();

            for (var i = 1; i < lines.Length; i++)
            {
                try
                {
                    var fields = lines[i].Split(';');

                    if (fields.Length >= 2)
                    {
                        var id = int.Parse(fields[0].Trim());
                        var name = fields[1].Trim();

                        var movie = new Movie
                        {
                            MovieID = id,
                            MovieTitle = name
                        };

                        movies.Add(movie);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping invalid line: {lines[i]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return movies;
        }
    }
}
