using System;
using System.ComponentModel.Composition;

namespace MefContribDemo.Filtering
{
    public interface ISharedPart { }

    [Export(typeof(ISharedPart))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SharedPart : ISharedPart, IDisposable
    {
        public SharedPart()
        {
            Console.WriteLine("SharedPart()");
        }

        public void Dispose()
        {
            Console.WriteLine("SharedPart.Dispose()");
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
            Console.WriteLine("NonSharedPart()");
        }

        public void Dispose()
        {
            Console.WriteLine("NonSharedPart.Dispose()");
        }
    }

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RequestSpecificPart : IDisposable
    {
        [Import]
        public INonSharedPart Part { get; set; }

        public RequestSpecificPart()
        {
            Console.WriteLine("RequestSpecificPart()");
        }
        public void Dispose()
        {
            Console.WriteLine("RequestSpecificPart.Dispose()");
        }
    }
}