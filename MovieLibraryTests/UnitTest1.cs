using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieLibrary.Adapter;
using MovieLibrary.Controllers;
using MovieLibrary.Models;
using System.Collections.Generic;
using System.Net.Http;

namespace MovieLibraryTests
{
    [TestClass]
    public class MovieLibraryTests
    {
        static HttpClient _client = new HttpClient();
        MovieAdapter _movieAdapter = new MovieAdapter();
        ListController _listController = new ListController();
        MovieController _movieController = new MovieController();
        Movie movie = new Movie();
        DetailedMovie detailedMovie = new DetailedMovie();

        [TestMethod]
        public void calling_movie_adapter_should_return_combined_and_correct_list_with_correct_and_parsed_types()
        {
            // Arrange
            List<Movie> movieList = new List<Movie>();
            List<DetailedMovie> detailedMovieList = new List<DetailedMovie>();

            movieList.Add(new Movie { Id = "1", Title = "Testing 1", Rated = "1" });
            detailedMovieList.Add(new DetailedMovie { Id = "2", Title = "Testing 2", ImdbRating = 1.5m });

            var movieForComparrisonFromMovieList = movieList.Find(m => m.Id.Equals("1"));
            var detailedMovieBeforeParse = detailedMovieList.Find(m => m.Id.Equals("2"));

            // A cheeky Assert, only here to be able to Assert that the parsing actually worked as expected 
            // (DetailedMovie parsed to Movie).
            Assert.IsFalse(detailedMovieBeforeParse.GetType() == movieForComparrisonFromMovieList.GetType());

            // Act
            var movieTestList = _movieAdapter.ParseRatings(movieList, detailedMovieList);

            var listTestMovieOne = movieTestList.Find(m => m.Title.Equals("Testing 1"));
            // listTestMovieTwo used to be DetailedMovieType, should now be Movie
            var listTestMovieTwo = movieTestList.Find(m => m.Title.Equals("Testing 2"));

            // Assert
            // A few of the asserts might seem redundant, it's just there to be thorough. Hold on! :)
            Assert.IsNotNull(movieTestList);

            Assert.IsTrue(movieTestList.Count == 2);

            Assert.AreEqual(listTestMovieOne.Title, "Testing 1");
            Assert.AreEqual(listTestMovieTwo.Title, "Testing 2");

            // Here we're following up on the cheeky Assert from earlier, to see if the parsing worked as expected
            Assert.AreEqual(listTestMovieOne.GetType(), listTestMovieTwo.GetType());
        }

        [TestMethod]
        public void calling_execute_call_chain_in_list_controller_should_run_through_the_entire_process_without_failure()
        {
            // Only asserting the execute call chain with "allMovies" attribute, since the "getSingleMovieById" is the same, 
            // but with less functionality. Only have the test for "getSingleMovieById" to get coverage, otherwise it's unnecessary.

            // Arrange

            // Act
            var resultAscending = _listController.ExecuteCallChain(true, "allMovies");
            var resultDescending = _listController.ExecuteCallChain(false, "allMovies");
            var resultGetSingleMovieById = _listController.ExecuteCallChain(true, "getSingleMovieById");
            var resultIfNoInputIsGiven = _listController.ExecuteCallChain(true, "");

            // Assert
            // I could use 'expected' and 'actual', but I prefer it this way, since I find it clearer and easier to read.
            Assert.IsNotNull(resultAscending);
            Assert.IsNotNull(resultDescending);
            Assert.IsNotNull(resultGetSingleMovieById);
            Assert.IsNull(resultIfNoInputIsGiven);

            Assert.IsTrue(resultAscending.Count == 115);
            Assert.IsTrue(resultDescending.Count == 115);
            Assert.IsTrue(resultGetSingleMovieById.Count == 115);
        }

        [TestMethod]
        public void calling_movie_controller_should_return_valid_lists()
        {
            // Arrange


            // Act
            var testGetAllMovies = _movieController.GetAllMovies();
            var testMovieById = _movieController.GetMovieById("1");

            // Assert
            Assert.IsNotNull(testGetAllMovies);
            Assert.IsNotNull(testMovieById);

            Assert.IsTrue(testGetAllMovies.Count == 115);

            Assert.IsTrue(testMovieById.Id.Equals("1"));
            Assert.IsTrue(testMovieById.Title != null && !testMovieById.Title.Equals(""));
            Assert.IsNotNull(testMovieById.Title.Equals("Black Panther"));
            Assert.IsTrue(testMovieById.Rated != null && !testMovieById.Rated.Equals(""));
            Assert.IsTrue(testMovieById.Rated.Equals("7,0"));
        }
    }
}
