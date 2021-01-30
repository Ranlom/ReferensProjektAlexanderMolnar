using MovieLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibrary.Adapter
{
    public class MovieAdapter
    {

        public List<Movie> ParseRatings(List<Movie> topListMovieList, List<DetailedMovie> detailedMovieList)
        {
            List<Movie> parsedMovieList = new List<Movie>();

            foreach (var detailedMovie in detailedMovieList)
            {
                if (!topListMovieList.Any(m => m.Title == detailedMovie.Title))
                {
                    var adaptedMovie = new Movie
                    {
                        Id = detailedMovie.Id,
                        Title = detailedMovie.Title,
                        Rated = detailedMovie.ImdbRating.ToString(),
                    };

                    topListMovieList.Add(adaptedMovie);
                }
            }

            return topListMovieList;
        }
    }
}
