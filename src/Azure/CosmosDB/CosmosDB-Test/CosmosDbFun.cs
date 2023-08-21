using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB_Test
{
    class CosmosDbFun
    {
        private const string EndpointUrl = "https://localhost:8081/";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private DocumentClient _client;
        

        public async Task Init()
        {
            try
            {
                _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = "TestDB" });

                DocumentCollection myCollection = new DocumentCollection();
                myCollection.Id = "Families";
                myCollection.PartitionKey.Paths.Add("/type");

                var x = await _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("TestDB"),
                    myCollection,
                    new RequestOptions { OfferThroughput = 2500 });

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Run()
        {
            var collection = UriFactory.CreateDocumentCollectionUri("TestDB", "Families");
            var sql = new SqlQuerySpec("select * from f");
            //var sql = new SqlQuerySpec("SELECT {\"Name\":f.id, \"City\":f.address.city} AS Family FROM Families f WHERE f.address.city = f.address.state AND f.type = \"familiy\"");
            //var sql = new SqlQuerySpec("SELECT f.id as name, f.address.city as city FROM Families f WHERE f.address.city = f.address.state AND f.type = \"familiy\"");
            var results = _client.CreateDocumentQuery(collection, sql, new FeedOptions() { EnableCrossPartitionQuery = true }).ToList();
            if (results.Count == 0)
            {
                var data = File.ReadAllText("./Data/Data1.json");
                var obj = JsonConvert.DeserializeObject(data);

                var result = await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "Families"),
                   obj);

                data = File.ReadAllText("./Data/Data2.json");
                obj = JsonConvert.DeserializeObject(data);

                result = await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "Families"),
                   obj);
            }
            else
            {
                foreach (var item in results)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Single doc:");
            var doc = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri("TestDB", "Families", "AndersenFamily"), new RequestOptions() { PartitionKey = new PartitionKey("familiy") });
            Console.WriteLine(doc.Resource);
        }
    }
}
