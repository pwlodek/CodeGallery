using System;
using System.ComponentModel.Composition;

namespace MefContribDemo.Interception
{
    public interface IBar : IStartable
    {
        void Foo();
    }

    [Export(typeof(IBar))]
    public class Bar : IBar
    {
        public Bar()
        {
            Console.WriteLine("Bar()");
        }

        public void Start()
        {
            Console.WriteLine("Bar.Start()");
        }

        public void Foo()
        {
            Console.WriteLine("Bar.Foo()");
        }
    }

    public interface IFoo : IStartable
    {
        void Bar();
    }

    [Export(typeof(IFoo))]
    [ExportMetadata("Log", true)]
    public class Foo : IFoo
    {
        public Foo()
        {
            Console.WriteLine("Foo()");
        }
        public void Bar()
        {
            Console.WriteLine("Foo.Bar()");
        }

        public void Start()
        {
            Console.WriteLine("Foo.Start()");
        }
    }
}