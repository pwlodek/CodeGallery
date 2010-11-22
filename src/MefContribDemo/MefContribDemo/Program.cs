using System;
using MefContribDemo.Filter;
using MefContribDemo.Generics;
using MefContribDemo.Interception;

namespace MefContribDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new GenericsDemo().Run();

            new FilteringDemo().Run();
            
            new InterceptionDemo().Run();
        }
    }
}
