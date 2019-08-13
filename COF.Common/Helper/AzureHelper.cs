using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace Common.Helper
{
    public class AzureHelper
    {
        #region blob client

        private static CloudBlobClient _cloudBlobClient;

        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                if (_cloudBlobClient == null)
                {
                    var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
                    // Create blob client and return reference to the container
                    var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    _cloudBlobClient = blobStorageAccount.CreateCloudBlobClient();
                }

                return _cloudBlobClient;
            }


        }

        #endregion

        #region container
        /// <summary>
        /// avatar container, public
        /// </summary>
        private static CloudBlobContainer _dailyOrderExportContainer;

        /// <summary>
        /// avatar container, public
        /// </summary>
        public static CloudBlobContainer DailyOrderExportContainer
        {
            get
            {
                if (_dailyOrderExportContainer == null)
                {
                    var dailyOrderExportFolder = ConfigurationManager.AppSettings["DailyOrderExport"];
                    // Retrieve a reference to a container. 
                    _dailyOrderExportContainer = CloudBlobClient.GetContainerReference(dailyOrderExportFolder);
                    // Create the container if it doesn't already exist.
                    _dailyOrderExportContainer.CreateIfNotExists();

                }

                return _dailyOrderExportContainer;
            }

        }

        #endregion
    }
}