using System;
using MefContribDemo.Filtering;
using MefContribDemo.Generics;
using MefContribDemo.Interception;

namespace MefContribDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new FilteringScenario().Run();
            new GenericsScenario().Run();
            new InterceptionScenario().Run();
        }
    }
}
