using System;
using System.Collections.Generic;
using System.Linq;

namespace ConventionCatalogDemo
{
    /********** INTERFACE **********/

    public interface IMovieLister
    {
        IEnumerable<Movie> GetMovies();

        IEnumerable<Movie> GetMoviesByName(string name);
    }
    
    /********** IMPLEMENTATION **********/

    public class MovieLister : IMovieLister
    {
        private readonly ILogger _logger;

        public MovieLister(ILogger logger)
        {
            _logger = logger;
        }

        public IMovieProvider[] Providers { get; set; }

        public IEnumerable<Movie> GetMovies()
        {
            var movies = Providers
                .SelectMany(movieProvider => movieProvider.GetMovies())
                .ToList();
            
            _logger.Log(string.Format("Loaded {0} movies.", movies.Count));

            return movies;
        }

        public IEnumerable<Movie> GetMoviesByName(string name)
        {
            var movies = GetMovies()
                .Where(m => m.Name.Contains(name))
                .ToList();

            _logger.Log(string.Format("Found {0} movies matching '{1}'.", movies.Count, name));

            return movies;
        }
    }

    public class Movie
    {
        public string Name { get; set; }
    }
}