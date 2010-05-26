using System;
using System.Globalization;
using LocalizationDemo.Properties;
using ClassLibrary1;
using System.Threading;

namespace LocalizationDemo
{
    class Program
    {
        private static void Main(string[] args)
        {
            var testClass = new TestClass();

            // Default language
            Console.WriteLine(Resources.String1);
            Console.WriteLine(testClass.Hello());

            // British
            SetLanguage("en-GB");
            Console.WriteLine(Resources.String1);
            Console.WriteLine(testClass.Hello());

            // Polish
            SetLanguage("pl-PL");
            Console.WriteLine(Resources.String1);
            Console.WriteLine(testClass.Hello());
        }

        private static void SetLanguage(string lang)
        {
            var ci = new CultureInfo(lang);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
