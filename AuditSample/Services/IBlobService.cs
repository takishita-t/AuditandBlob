
namespace AuditSample.Services
{
    public interface IBlobService
    {
        Task DownloadAsync(string downloadContainerName, string downloadFileName, string saveFilePath);
        Task UploadAsync(string uploadContainerName, string uploadFileName, string uploadFilePath);
        Task CopyAsync(string copyContainerName, string copyFromFileName, string copyToFileName);
    }
}