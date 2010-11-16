using System;
using System.ComponentModel.Composition;

namespace MefContribDemo.Filter
{
    public interface ISharedPart { }

    [Export(typeof(ISharedPart))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SharedPart : ISharedPart, IDisposable
    {
        public SharedPart()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SharedPart()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Dispose()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SharedPart.Dispose()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    public interface INonSharedPart { }

    [Export(typeof(INonSharedPart))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class NonSharedPart : INonSharedPart, IDisposable
    {
        [Import]
        public ISharedPart Part { get; set; }

        public NonSharedPart()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NonSharedPart()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Dispose()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NonSharedPart.Dispose()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    public interface IRequestSpecificPart { }

    [Export(typeof(IRequestSpecificPart))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RequestSpecificPart : IRequestSpecificPart, IDisposable
    {
        [Import]
        public INonSharedPart Part { get; set; }

        public RequestSpecificPart()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RequestSpecificPart()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void Dispose()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RequestSpecificPart.Dispose()");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}