using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageDemo
{
    class RedisDemo
    {
        public static void Demo()
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("piotrscache.redis.cache.windows.net:6380,password=fu7iB/6b8/4SwzXAbnUr3lm4YSD/aR938JnB/tN773w=,ssl=True,abortConnect=False");
            var database = connection.GetDatabase();

            var s = database.StringGet("key1");
            if (string.IsNullOrWhiteSpace(s))
            {
                database.StringSet("key1", "this string is cached");
                
                Console.WriteLine("Saved string from to redis");

                s = database.StringGet("key1");

                Console.WriteLine("Retrieved: " + s);
            }

            else
            {
                Console.WriteLine("Cache contains: " + s);
                database.StringSet("key1", $"this string is cached {DateTime.Now}");
            }
        }
    }
}
