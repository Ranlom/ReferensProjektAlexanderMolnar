using MovieLibrary.Adapter;
using MovieLibrary.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace MovieLibrary.Controllers
{
    public class ListController
    {
        static HttpClient _client = new HttpClient();
        MovieAdapter _movieAdapter = new MovieAdapter();

        string firstApi = "https://ithstenta2020.s3.eu-north-1.amazonaws.com/topp100.json";
        string secondApi = "https://ithstenta2020.s3.eu-north-1.amazonaws.com/detailedMovies.json";

        public dynamic ExecuteCallChain(bool ascending, string operation)

        {
            if (operation.Equals("allMovies"))
            {
                var firstApiCall = CallApi(firstApi);
                var secondApiCall = CallApi(secondApi);
                List<Movie> firstApiCallDeserialized = DeserializeJson("firstApi", firstApiCall);
                List<DetailedMovie> secondApiCallDeserialized = DeserializeJson("secondApi", secondApiCall);
                var parsedAndSortedCombinedList = _movieAdapter.ParseRatings(firstApiCallDeserialized, secondApiCallDeserialized);
                List<string> completeMovieList = SortListAndChangeToStringList(parsedAndSortedCombinedList, ascending);

                return completeMovieList;
            }
            else if (operation.Equals("getSingleMovieById"))
            {
                var firstApiCall = CallApi(firstApi);
                var secondApiCall = CallApi(secondApi);
                List<Movie> firstApiCallDeserialized = DeserializeJson("firstApi", firstApiCall);
                List<DetailedMovie> secondApiCallDeserialized = DeserializeJson("secondApi", secondApiCall);
                List<Movie> parsedAndSortedCombinedList = _movieAdapter.ParseRatings(firstApiCallDeserialized, secondApiCallDeserialized);

                return parsedAndSortedCombinedList;
            }

            return null;
        }

        public HttpResponseMessage CallApi(string url)
        {
            HttpResponseMessage apiCallResult;

            apiCallResult = _client.GetAsync(url).Result;

            return apiCallResult;
        }

        public dynamic DeserializeJson(string jsonSelect, HttpResponseMessage httpResponseFromSelectedJson)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            List<Movie> stringListOfMovies = new List<Movie>();
            List<DetailedMovie> stringListOfDetailedMovies = new List<DetailedMovie>();

            if (jsonSelect.Equals("firstApi"))
            {
                return JsonSerializer.Deserialize<List<Movie>>(new StreamReader(httpResponseFromSelectedJson.Content.ReadAsStream()).ReadToEnd(), jsonOptions);
            }
            else if (jsonSelect.Equals("secondApi"))
            {
                return JsonSerializer.Deserialize<List<DetailedMovie>>(new StreamReader(httpResponseFromSelectedJson.Content.ReadAsStream()).ReadToEnd(), jsonOptions);
            }

            return null;
        }

        public List<string> SortListAndChangeToStringList(List<Movie> parsedAndSortedCombinedList, bool ascending)
        {
            if (ascending)
            {
                parsedAndSortedCombinedList = parsedAndSortedCombinedList.OrderBy(e => e.Rated).ToList();
            }
            else
            {
                parsedAndSortedCombinedList = parsedAndSortedCombinedList.OrderByDescending(e => e.Rated).ToList();
            }

            var stringList = new List<string>();

            foreach (var movie in parsedAndSortedCombinedList)
            {
                stringList.Add(movie.Title);
            }

            return stringList;
        }
    }
}
