using log4net.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Appender
{
    
    public class AzureAppendBlobAppender : BufferingAppenderSkeleton
    {
        private CloudStorageAccount _account;
        private CloudBlobClient _client;
        private CloudBlobContainer _cloudBlobContainer;

        public string ConnectionStringName { get; set; }
        
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
        
        public string DirectoryName { get; set; }
        
        protected override void SendBuffer(LoggingEvent[] events)
        {
            CloudAppendBlob appendBlob = _cloudBlobContainer.GetAppendBlobReference(Filename(DirectoryName));

            if (!appendBlob.Exists())
            {
                appendBlob.CreateOrReplace();
            }

            StringBuilder sb = new StringBuilder();
            foreach (var loggingEvent in events)
            {
                sb.Append(RenderLoggingEvent(loggingEvent));
            }

            appendBlob.AppendText(sb.ToString());
        }

        private static string Filename(string directoryName)
        {
            return string.Format("{0}/{1}.log",  directoryName, DateTime.Today.ToString("yyyy_MM_dd", DateTimeFormatInfo.InvariantInfo));
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            _account = CloudStorageAccount.Parse(ConnectionString);
            _client = _account.CreateCloudBlobClient();
            _cloudBlobContainer = _client.GetContainerReference(ContainerName.ToLower());
            _cloudBlobContainer.CreateIfNotExists();
        }
    }
}