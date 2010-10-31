using System;
using System.ComponentModel.Composition;

namespace MefContribDemo.Interception
{
    #region Bar

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

        public bool IsStarted { get; private set; }

        public void Start()
        {
            IsStarted = true;
            Console.WriteLine("Bar.Start()");
        }

        public void Foo()
        {
            Console.WriteLine("Bar.Foo()");
        }
    }

    #endregion
    
    #region Foo

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
        
        public bool IsStarted { get; private set; }

        public void Bar()
        {
            Console.WriteLine("Foo.Bar()");
        }

        public void Start()
        {
            IsStarted = true;
            Console.WriteLine("Foo.Start()");
        }
    }

    #endregion
}