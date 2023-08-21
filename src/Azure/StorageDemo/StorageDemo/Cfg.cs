using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageDemo
{
    public class Cfg
    {
        public Cfg()
        {
        }

        public static CloudStorageAccount GetAccount()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=demostoragepiotr;AccountKey=7z+zc3XtgnbqGERzgMJnF5j8Y4xGpALG5FXeBM1CMa76DjZImaFWuEOBgkUWM/kojFs4tNSsFultsmqoEGHBaA==;EndpointSuffix=core.windows.net");
            return storageAccount;
        }

		/// <summary>
		/// Create a table for the sample application to process messages in. 
		/// </summary>
		/// <returns>A CloudTable object</returns>
		public static async Task<CloudTable> CreateTableAsync(string tableName)
		{
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = GetAccount();

			// Create a table client for interacting with the table service
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			Console.WriteLine("Create a Table for the demo");

			// Create a table client for interacting with the table service 
			CloudTable table = tableClient.GetTableReference(tableName);
			try
			{
				if (await table.CreateIfNotExistsAsync())
				{
					Console.WriteLine("Created Table named: {0}", tableName);
				}
				else
				{
					Console.WriteLine("Table {0} already exists", tableName);
				}
			}
			catch (StorageException)
			{
				Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
				Console.ReadLine();
				throw;
			}

			Console.WriteLine();
			return table;
		}
    }
}
