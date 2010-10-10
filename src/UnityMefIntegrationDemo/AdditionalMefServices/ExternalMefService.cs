using System;
using System.ComponentModel.Composition;

namespace AdditionalMefServices
{
    [Export(typeof(IMefService))]
    public class ExternalMefService : IMefService
    {
        public void Bar()
        {
            Console.WriteLine("ExternalMefService.Bar()");
        }
    }
}