using System;

namespace StorageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var demo = new GettingStartedQueues();
            //demo.RunQueueStorageOperationsAsync();
            //StorageBlobDemo.CallBlobGettingStartedSamples();
            //StorageTableDemo.RunSamples();
            RedisDemo.Demo();
            Console.WriteLine("End demo.");
            Console.ReadKey();
        }
    }
}
