using Backend.Data.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class CosmosDbProvider
    {
        private const string EndpointUrl = "https://pwlodek.documents.azure.com:443/";
        private const string PrimaryKey = "p9ofzlF4UU9ikJ90UIPuU4onJFSofpwIy9yQQlsApAhHH15WG6FUToSGtOm4BV1Lri3jjU9VZIBFBFeWQpxYGw==";
        private DocumentClient _client;
        private bool _initialized;

        public CosmosDbProvider()
        {

        }

        private async Task InitAsync()
        {
            if (_initialized)
            {
                return;
            }

            try
            {
                _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = "TestDB" });

                DocumentCollection myCollection = new DocumentCollection();
                myCollection.Id = "TodoItems";
                myCollection.PartitionKey.Paths.Add("/UserName");

                await _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("TestDB"),
                    myCollection,
                    new RequestOptions { OfferThroughput = 400 });

                _initialized = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TodoItem> GetTodoItem(string id)
        {
            await InitAsync();

            var result = await _client.ReadDocumentAsync<TodoItem>(
                   UriFactory.CreateDocumentUri("TestDB", "TodoItems", id),
                   new RequestOptions { PartitionKey = new PartitionKey("Piotr") });

            return result.Document;
        }

        public async Task AddTodoItem(TodoItem item)
        {
            await InitAsync();

            var result = await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "TodoItems"),
                   item);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            await InitAsync();

            var result = _client.CreateDocumentQuery<TodoItem>(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "TodoItems")).ToList();

            return result;
        }
    }
}