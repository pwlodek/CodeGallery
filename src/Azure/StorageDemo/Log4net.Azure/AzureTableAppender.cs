using log4net.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Appender
{
    public class AzureTableAppender : BufferingAppenderSkeleton
    {
        private CloudStorageAccount _account;
        private CloudTableClient _client;
        private CloudTable _cloudTable;

        public string ConnectionStringName { get; set; }

        public string ConnectionString { get; set; }

        public string TableName { get; set; }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            var groups = events.GroupBy(e => e.LoggerName);

            foreach (var group in groups)
            {
                TableBatchOperation batchOperation = new TableBatchOperation();

                foreach (var e in group)
                {
                    LoggingEntity entity = new LoggingEntity(e) { Message = RenderLoggingEvent(e) };
                    batchOperation.Insert(entity);
                }

                _cloudTable.ExecuteBatch(batchOperation);
            }            
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            _account = CloudStorageAccount.Parse(ConnectionString);
            _client = _account.CreateCloudTableClient();
            _cloudTable = _client.GetTableReference(TableName);
            _cloudTable.CreateIfNotExists();
        }

        internal sealed class LoggingEntity : TableEntity
        {
            public LoggingEntity()
            {

            }

            public LoggingEntity(LoggingEvent e)
            {
                Level = e.Level.ToString();
                ThreadName = e.ThreadName;
                EventTimeStamp = e.TimeStamp;

                PartitionKey = e.LoggerName;
                RowKey = Guid.NewGuid().ToString();
            }

            public DateTime EventTimeStamp { get; set; }

            public string ThreadName { get; set; }

            public string Level { get; set; }

            public string Message { get; set; }
        }
    }
}