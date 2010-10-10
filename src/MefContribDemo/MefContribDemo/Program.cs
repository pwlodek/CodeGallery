using System;

namespace MefContribDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new FactoryDemo().Run();
            new GenericsDemo().Run();
            new InterceptionDemo().Run();
        }
    }
}
