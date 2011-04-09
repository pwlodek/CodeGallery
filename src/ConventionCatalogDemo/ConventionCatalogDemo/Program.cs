using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting.Conventions;
using MefContrib.Hosting.Conventions.Configuration;

namespace ConventionCatalogDemo
{
    public class Program
    {
        [Import]
        public IMovieLister MovieLister { get; set; }

        public static void Main(string[] args)
        {
            var p = new Program();
            p.Init();
            p.Run();
        }

    private void Init()
    {
        var conventionCatalog = new ConventionCatalog(
            new ConfigurationPartRegistry("mef.configuration"),
            new MoviePartRegistry());
        var container = new CompositionContainer(conventionCatalog);

        // Composing this part will inject MovieLister property
        container.ComposeParts(this);
    }

        private void Run()
        {
            var movies = MovieLister.GetMoviesByName("Movie");
            foreach (var movie in movies)
            {
                Console.WriteLine(movie.Name);
            }
        }
    }

    public class MoviePartRegistry : PartRegistry
    {
        public MoviePartRegistry()
        {
            // Apply the conventions to all types int the specified assembly
            Scan(c => c.Assembly(typeof(Program).Assembly));
            
            Part<MovieLister>()
                .MakeShared() // make this part shared
                .ExportAs<IMovieLister>() // and export it with contract type IMovieLister
                .ImportConstructor() // use constructor injection
                .Imports(x =>
                {
                    x.Import<MovieLister>() // import on part MovieLister
                        .Member(m => m.Providers) // member named 'Providers'
                        .ContractType<IMovieProvider>(); // with contract type IMovieProvider
                });

            //Part()
            //    .ForTypesAssignableFrom<IMovieProvider>()
            //    .ExportAs<IMovieProvider>();

            Part<LoggerImpl>()
                .MakeShared()
                .ExportAs<ILogger>();
        }
    }
}
