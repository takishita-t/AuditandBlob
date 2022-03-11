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
            // Azuru Storage アカウント接続文字列
            string connectionString = _blobSetting.Value.ConnectionString;

            // Azuru Storage コンテナ名
            string containerName = downloadContainerName;

            // コンテナ取得
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            //blob内、ダウンロードするファイルの名前
            string downloadBlobname = downloadFileName;

            //保存先パスとファイルの保存名
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
            // Azuru Storage アカウント接続文字列
            string connectionString = _blobSetting.Value.ConnectionString;

            // Azuru Storage コンテナ名
            string containerName = uploadContainerName;

            // コンテナ取得
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            //アップロードファイルのコンテナ内の名前
            string uploadBlobname = uploadFileName;

            //アップロード元のファイルパス
            string uploadFilepath = uploadFilePath;

            //コンテナなければ作る
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
            // Azuru Storage アカウント接続文字列
            string connectionString = _blobSetting.Value.ConnectionString;

            // Azuru Storage コンテナ名
            string containerName = copyContainerName;

            // コンテナ取得
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            // コピー元ファイルパス
            string copyFrom = copyFromFileName;

            // コピー先ファイルパス
            string copyTo = copyToFileName;

            // コピー元blob取得
            BlobClient copyFromBlob = container.GetBlobClient(copyFrom);

            // コピー先blob取得
            BlobClient copyToBlob = container.GetBlobClient(copyTo);

            try
            {
                // コピー実行
                copyToBlob.StartCopyFromUri(copyFromBlob.Uri);

                // コピー元削除
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
