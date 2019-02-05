using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.Hadoop.Avro;
using Microsoft.Hadoop.Avro.Schema;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Avro
{
    class CleanerClient
    {
        public CloudBlobContainer Container { get; set; }
        public ILogger Log { get; set; }

        /// <summary>
        /// Constructor for cleaning
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="containerName"></param>
        public CleanerClient(string connectionString, string containerName, ILogger log)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            Container = cloudBlobContainer;
            Log = log;
        }

        /// <summary>
        /// Runs cleaning on Json-file in blob storage
        /// </summary>
        /// <param name="blobPath"></param>
        public void RunCleaning(string blobFileName, List<string> columns)
        {
            try
            {
                Log.LogWarning($"Started cleaning on {blobFileName}");
                int page = 0;
                //int batchSize = 5000;
                //int totalRowcount = 0;
                //JArray dataset = new JArray();
                List<dynamic> dataset = new List<dynamic>();
                string blobPath = GetInputPath(blobFileName);
                CloudBlob blob = Container.GetBlobReference(blobPath);
                bool exits = blob.Exists();

                using (Stream stream = blob.OpenRead())
                {
                    using (var reader = AvroContainer.CreateGenericReader(stream))
                    {
                        while (reader.MoveNext())
                        {
                            foreach (dynamic record in reader.Current.Objects)
                            {
                                page++;
                                string caseNo = record.CASE_NO.ToString();
                                Log.LogInformation($"Page: {page}");
                                Log.LogInformation($"Case: {caseNo}");

                            }
                        }
                    }

                }


            }
            catch (Exception e)
            {
                Log.LogError($"Error: {e}");
                throw e;
            }

        }


        /// <summary>
        /// Deleting existing blobs in the container
        /// </summary>
        /// <param name="container"></param>
        public List<string> DeleteBlobs()
        {
            Log.LogWarning("Started deleting blobs");
            var blobs = Container.ListBlobs(useFlatBlobListing: true).OfType<CloudBlockBlob>().ToList();
            List<string> blobNames = new List<string>();
            foreach (var item in blobs)
            {
                var blobName = item.Name;
                blobNames.Add(blobName);
                Console.WriteLine(blobName);
                var blockBlob = Container.GetBlockBlobReference(blobName);
                blockBlob.DeleteIfExists();
            }
            return blobNames;

        }

        /// <summary>
        /// Cleaning columns
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columns"></param>
        private dynamic CleanColumns(dynamic data, List<string> columns)
        {
            foreach (var column in columns)
            {
                string sentenceDirty = data[column];
                string sentenceClean = null;
                data[column] = sentenceClean;
            }
            return data;
        }


        /// <summary>
        /// Gets the path of the output file
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private string GetOutputPath(string inputPath, int page)
        {
            return inputPath.Replace("Input", "Output").Replace(".avro", $"_{page}.json");
        }

        /// <summary>
        /// Gets the path to the input file in blob storage
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string GetInputPath(string blobFileName)
        {
            return $"Input/{blobFileName}";
        }


        private void UploadToBlob(List<dynamic> data, string outputPath)
        {
            try
            {
                Log.LogWarning($"Uploading {outputPath}");
                //string contentType = "application/json";
                //cloudBlockBlob.Properties.ContentType = contentType;
                CloudBlockBlob cloudBlockBlob = Container.GetBlockBlobReference(outputPath);

                using (var ms = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(ms);
                    writer.Write(data);
                    writer.Flush();
                    ms.Position = 0;
                    cloudBlockBlob.UploadFromStream(ms);
                    GC.Collect();
                }
            }
            catch (Exception e)
            {
                Log.LogError(e.Message + e.StackTrace);
                throw e;
            }

        }

    }
}
