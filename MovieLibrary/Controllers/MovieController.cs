using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace MovieLibrary.Controllers
{
    [ApiController,
    Route("api/[controller]")]
    public class MovieController
    {
        static HttpClient _client = new HttpClient();
        ListController _listController = new ListController();

        [HttpGet]
        [Route("/getAllMovies")]
        public List<string> GetAllMovies([FromQuery] bool ascending = true)
        {
            return _listController.ExecuteCallChain(ascending, "allMovies");
        }

        [HttpGet]
        [Route("/getMovieById")]
        public Movie GetMovieById(string id)
        {
            var movieList = _listController.ExecuteCallChain(true, "getSingleMovieById");

            foreach (var movie in movieList)
            {
                if (movie.Id.Equals((id)))
                {
                    return movie;
                }
            }

            return new Movie { Id = "404", Title = "Movie not found. Try another id!", Rated = "Very bad. Horrible." };
        }
    }
}