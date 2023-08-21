using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDB_Test
{
    class CosmosDbProvider
    {
        private const string EndpointUrl = "https://pwlodek.documents.azure.com:443/";
        private const string PrimaryKey = "2QCRBXjV5olsHTTn3swHFQNR542h04sJEcWyTXviizC048T9fjwMbkbWPBThwfVoaIIVgXWUrSGLsDkJuhnQug==";
        private DocumentClient _client;

        public CosmosDbProvider()
        {

        }        

        public async Task Init()
        {
            try
            {
                _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

                await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = "TestDB" });

                DocumentCollection myCollection = new DocumentCollection();
                myCollection.Id = "TodoItems";
                myCollection.PartitionKey.Paths.Add("/category");

                await _client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("TestDB"),
                    myCollection,
                    new RequestOptions { OfferThroughput = 400 });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TodoItem> GetTodoItem(string id)
        {
            var result = await _client.ReadDocumentAsync<TodoItem>(
                   UriFactory.CreateDocumentUri("TestDB", "TodoItems", id), new RequestOptions { PartitionKey = new PartitionKey("personal") });

            return result.Document;
        }

        public async Task AddTodoItem(TodoItem item)
        {
            var result = await _client.CreateDocumentAsync(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "TodoItems"),
                   item);
        }

        public IEnumerable<TodoItem> GetTodoItems()
        {
            var result =  _client.CreateDocumentQuery<TodoItem>(
                   UriFactory.CreateDocumentCollectionUri("TestDB", "TodoItems")).ToList();
           

            return result;
        }
    }
}
