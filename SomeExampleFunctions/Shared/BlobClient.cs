using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace SomeExampleFunctions.Shared
{
    public class BlobClient
    {
        private readonly BlobContainerClient _containerClient;

        public BlobClient()
        {
            var blobConnectionString = Configuration.ValueOf("StorageConnectionString");
            var blobContainerName = Configuration.ValueOf("StorageContainerName");
            var blobClient = new BlobServiceClient(blobConnectionString);
            _containerClient = blobClient.GetBlobContainerClient(blobContainerName);
        }

        public async Task<string> WriteFile(string fileName, string content)
        {
            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await _containerClient.UploadBlobAsync(fileName, contentStream);
            return new Uri(_containerClient.Uri, fileName).ToString();
        }
    }
}
