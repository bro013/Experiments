using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob; 

namespace DeleteBlobs
{
    class Program

    {
        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=roslandenstorage;AccountKey=redzOAF6y0zbOdpZxyqrMhckuXIJfNc9Lpr6lxcJpu4aN5teL6YklnO8gOEHvjYyJ6tnstqNqpdGEj0zfwrcvA==;EndpointSuffix=core.windows.net";
        private static readonly string _blobName = "test";
        public static void Main(string[] args)
        {
            CloudBlobContainer container = GetContainer(_connectionString, _blobName);
            DeleteBlobsInContainer(container);
        }


        private static void DeleteBlobsInContainer(CloudBlobContainer container)
        {
            var blobs = container.ListBlobsSegmentedAsync(null).GetAwaiter().GetResult().Results;
            foreach(var item in blobs)
            {
                string blobUri = item.Uri.ToString();
                Console.WriteLine(blobUri);
                var blockBlob = container.GetBlockBlobReference(blobUri);
                bool exist = blockBlob.ExistsAsync().GetAwaiter().GetResult();
                Console.WriteLine(exist);
                blockBlob.DeleteIfExistsAsync().Wait();
                Console.WriteLine(blockBlob.IsDeleted);

            }

        }

        private static CloudBlobContainer GetContainer(string connectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.FetchAttributesAsync().Wait();
            return container;
        }
    }
}

