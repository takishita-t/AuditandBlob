using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace AuditSample.Services
{
    public class BlobSetting
    {
        public string ConnectionString { get; set; }
    }

    public class Download
    {
        public string DownloadBlobName { get; set; }
        public string DownloadFileName { get; set; }
        public string SavePath { get; set; }
    }

    public class Upload
    {
        public string UploadContainerName { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFilePath { get; set; }
    }

    public class Copy
    {
        public string CopyContainerName { get; set; }
        public string CopyFromFileName { get; set; }
        public string CopyToFileName { get; set; }

    }

    public class BlobService : IBlobService
    {
        private readonly IOptions<BlobSetting> _blobSetting;

        public BlobService(IOptions<BlobSetting> blobSetting)
        {
            _blobSetting = blobSetting;
        }
        public async Task DownloadAsync(string downloadContainerName, string downloadFileName, string saveFilePath)
        {
            string connectionString = _blobSetting.Value.ConnectionString;

            string containerName = downloadContainerName;

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            string downloadBlobname = downloadFileName;

            string downloadFilepath = saveFilePath;

            if (!container.Exists())
            {
                container.Create();
            }

            try
            {
                BlobClient blob = container.GetBlobClient(downloadBlobname);

                using (var fileStream = File.OpenWrite(downloadFilepath))
                {
                    blob.DownloadTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }

        }

        public async Task UploadAsync(string uploadContainerName, string uploadFileName, string uploadFilePath)
        {
            string connectionString = _blobSetting.Value.ConnectionString;

            string containerName = uploadContainerName;

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            string uploadBlobname = uploadFileName;

            string uploadFilepath = uploadFilePath;

            if (!container.Exists())
            {
                container.Create();
            }
            try
            {
                BlobClient blob = container.GetBlobClient(uploadBlobname);

                using (var fileStream = File.OpenRead(uploadFilepath))
                {
                    blob.Upload(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }

        }

        public async Task CopyAsync(string copyContainerName, string copyFromFileName, string copyToFileName)
        {
            string connectionString = _blobSetting.Value.ConnectionString;

            string containerName = copyContainerName;

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            string copyFrom = copyFromFileName;

            string copyTo = copyToFileName;

            BlobClient copyFromBlob = container.GetBlobClient(copyFrom);

            BlobClient copyToBlob = container.GetBlobClient(copyTo);

            try
            {
                copyToBlob.StartCopyFromUri(copyFromBlob.Uri);

                copyFromBlob.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }

        }
    }
}
