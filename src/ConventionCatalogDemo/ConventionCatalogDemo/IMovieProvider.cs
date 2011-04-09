using System.Collections.Generic;

namespace ConventionCatalogDemo
{
    /********** INTERFACE **********/

    public interface IMovieProvider
    {
        IEnumerable<Movie> GetMovies();
    }

    /********** IMPLEMENTATION **********/

    public class MovieProvider1 : IMovieProvider
    {
        public IEnumerable<Movie> GetMovies()
        {
            yield return new Movie { Name = "Movie 1" };
            yield return new Movie { Name = "Movie 2" };
        }
    }

    public class MovieProvider2 : IMovieProvider
    {
        public IEnumerable<Movie> GetMovies()
        {
            yield return new Movie { Name = "Movie 3" };
            yield return new Movie { Name = "Movie 4" };
        }
    }
}