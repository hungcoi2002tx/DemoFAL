using Amazon.S3.Model;
using Amazon.Util.Internal;

namespace UserRekongition.Services.IServices
{
    public interface IS3Service
    {
        Task<bool> IsExistBudget(string budgetName);

        Task<bool> AddFileToS3Async(IFormFile file, string imageName, string bucketName);
    }
}
