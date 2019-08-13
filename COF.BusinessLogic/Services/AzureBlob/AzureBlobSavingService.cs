using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.AzureBlob
{
    public interface IAzureBlobSavingService
    {
        string SavingFileToAzureBlob(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer);

        string SavingFileToAzureBlobAndReturnUrl(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer);

        Task<string> SavingFileToAzureBlobAsync(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer);

        Task<string> RenameAzureUrlAsync(CloudBlobContainer cloudBlobContainer, string oldFilename, string newFilename);

        Task DeleteFilesAsync(CloudBlobContainer cloudBlobContainer, IEnumerable<string> filenames);
    }
    class AzureBlobSavingService : IAzureBlobSavingService
    {
        public string SavingFileToAzureBlob(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file and return file detail
            var blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(bytes, 0, bytes.Length);
            return name;
        }

        public string SavingFileToAzureBlobAndReturnUrl(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file and return file detail
            var blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(bytes, 0, bytes.Length);
            return blob.Uri.AbsoluteUri;
        }

        public Task<string> SavingFileToAzureBlobAsync(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file
            var blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(bytes, 0, bytes.Length);

            //return file detail
            return
                Task.FromResult(name);
        }

        public async Task<string> RenameAzureUrlAsync(CloudBlobContainer cloudBlobContainer, string oldFilename, string newFilename)
        {
            var container = cloudBlobContainer;
            var oldBlob = container.GetBlockBlobReference(oldFilename);
            var newBlob = container.GetBlockBlobReference(newFilename);

            using (var stream = new MemoryStream())
            {
                oldBlob.DownloadToStream(stream);
                stream.Seek(0, SeekOrigin.Begin);
                newBlob.Properties.ContentType = oldBlob.Properties.ContentType;
                newBlob.UploadFromStream(stream);
                await oldBlob.DeleteAsync();
            }
            return await Task.FromResult(newFilename);
        }

        public async Task DeleteFileAsync(CloudBlobContainer cloudBlobContainer, string filename)
        {
            var container = cloudBlobContainer;
            var blob = container.GetBlockBlobReference(filename);
            await blob.DeleteAsync();
        }

        public async Task DeleteFilesAsync(CloudBlobContainer cloudBlobContainer, IEnumerable<string> filenames)
        {
            var container = cloudBlobContainer;
            foreach (var filename in filenames)
            {
                var blob = container.GetBlockBlobReference(filename);
                await blob.DeleteAsync();
            }
        }

        #region Private Method
        /// <summary>  
        /// resize an image and maintain aspect ratio  
        /// </summary>  
        /// <param name="image">image to resize</param>  
        /// <param name="newWidth">desired width</param>  
        /// <param name="newHeight">max height</param>  
        /// <returns>resized image</returns>  
        private Image Resize(Image image, int newWidth, int newHeight)
        {
            Image thumb = image.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);
            return thumb;
        }

        /// <summary>
        /// convert byte array to image
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// convert image to byte array when don't know image's format input
        /// </summary>
        /// <param name="imageIn"></param>
        /// <param name="imageSource"></param>
        /// <returns></returns>
        private byte[] ImageToByteArray(Image imageIn, Image imageSource)
        {
            MemoryStream ms = new MemoryStream();

            if (imageSource.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else if (imageSource.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
            else if (imageSource.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            else if (imageSource.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            }
            else if (imageSource.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
            }
            else
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            return ms.ToArray();
        }


        #endregion

    }
}