using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB_Test
{
    class TodoItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    class Program
    {

        public async void Run()
        {
            CosmosDbProvider provider = new CosmosDbProvider();
            Random r = new Random();

            try
            {
                await provider.Init();

                var item = await provider.GetTodoItem("1");
                Console.WriteLine($"Todo Item {item.Name}");

                await provider.AddTodoItem(new TodoItem { Id = r.Next().ToString(), Name = "jakis name", Category = "groceris", Description = "bam ham tam" });

                var list = provider.GetTodoItems();

                foreach (var todoItem in list)
                {
                    Console.WriteLine(todoItem.Name);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async void Run2()
        {
            var p = new CosmosDbFun();
            await p.Init();
            await p.Run();

            Console.WriteLine("Hit any key to close.");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            new Program().Run2();
            Console.ReadKey();
        }
    }
}
