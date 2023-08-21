using Backend.Data;
using Backend.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            //p.AddNewItem();
            //p.GetItems();
            p.AddNewItem2();

            Console.ReadKey();
        }

        private int _count;

        private void AddNewItem()
        {
            while (true)
            {
                _count++;

                TodoItem item = new TodoItem()
                {
                    Name = $"Item {_count}",
                    Completed = false,
                    DueDate = DateTime.Now,
                    UserName = "Piotrek"
                };
                String json = JsonConvert.SerializeObject(item);
                HttpClient client = new HttpClient();

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                client.PostAsync("http://localhost:50682/api/items", content);

                Console.WriteLine("Sent item.");
                Console.ReadKey();
            }
        }

        private void AddNewItem2()
        {
            var qs = new QueueSender();
            while (true)
            {
                _count++;                

                TodoItem item = new TodoItem()
                {
                    Name = $"Item {_count}",
                    Completed = false,
                    DueDate = DateTime.Now,
                    UserName = "Piotrek"
                };

                Message m = new Message() { Operation = Operation.Add, Item = item };

                qs.Send(m);

                Console.WriteLine("Sent item.");
                Console.ReadKey();
            }
        }

        private async void GetItems()
        {
            var client = new HttpClient();
            var list = await client.GetStringAsync("http://localhost:50682/api/items");
            var items = JsonConvert.DeserializeObject<IList<TodoItem>>(list);

            Console.WriteLine($"Received {items.Count} items.");

            foreach (var item in items)
            {
                Console.WriteLine($"Item named {item.Name} due on {item.DueDate}");
            }
        }
    }
}
