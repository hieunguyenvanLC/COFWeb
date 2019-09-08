using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.Common.Helper
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

        #region daily export
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
        #region product image
        /// <summary>
        /// product image container, public
        /// </summary>
        private static CloudBlobContainer _productImageContainer;

        /// <summary>
        /// avatar container, public
        /// </summary>
        public static CloudBlobContainer ProductImageContainer
        {
            get
            {
                if (_productImageContainer == null)
                {
                    var productImageFolder = ConfigurationManager.AppSettings["Product"];
                    // Retrieve a reference to a container. 
                    _productImageContainer = CloudBlobClient.GetContainerReference(productImageFolder);
                    // Create the container if it doesn't already exist.
                    _productImageContainer.CreateIfNotExists();

                }

                return _productImageContainer;
            }

        }

        #endregion

        #region category image
        /// <summary>
        /// category image container, public
        /// </summary>
        private static CloudBlobContainer _categoryImageContainer;

        /// <summary>
        /// avatar container, public
        /// </summary>
        public static CloudBlobContainer CategoryImageContainer
        {
            get
            {
                if (_productImageContainer == null)
                {
                    var categoryImageFolder = ConfigurationManager.AppSettings["Category"];
                    // Retrieve a reference to a container. 
                    _categoryImageContainer = CloudBlobClient.GetContainerReference(categoryImageFolder);
                    // Create the container if it doesn't already exist.
                    _categoryImageContainer.CreateIfNotExists();

                }

                return _productImageContainer;
            }

        }

        #endregion
    }
}
