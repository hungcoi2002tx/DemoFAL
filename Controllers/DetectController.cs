using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using UserRekongition.Services.IServices;
using UserRekongition.Utils;

namespace UserRekongition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetectController : ControllerBase
    {
        private readonly IS3Service _S3Service;
        private readonly ICollectionService _CollectionService;
        private readonly string SystermId = "hungtest123";
        private readonly string BucketName = "hungtest123";

        public DetectController(ICollectionService collectionService, IS3Service s3Service)
        {
            _CollectionService = collectionService;
            _S3Service = s3Service;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectAsync(IFormFile file)
        {
            try
            {
                //check valid file
                #region check input
                file.ValidFile();
                #endregion
                //add s3
                var bucketExists = await _S3Service.IsExistBudget(BucketName);
                if (!bucketExists) return NotFound($"Bucket {BucketName} does not exist.");
                var fileName = Guid.NewGuid().ToString();
                var valueS3Return = await _S3Service.AddFileToS3Async(file, fileName, BucketName);

                //index faces
                var response = await _CollectionService.IndexFaceAsync(SystermId, BucketName, fileName);

                string result = string.Empty;
                foreach (var item in response.FaceRecords)
                {
                    var faceId = item.Face.FaceId;
                    var data = await _CollectionService.SearchUserByFaceIdsAsync(SystermId, faceId);
                    if(data.UserMatches != null && data.UserMatches.Count != 0)
                    {
                        var userId = data.UserMatches.First().User.UserId;
                        result = result + " " + userId;
                        //train again
                        await _CollectionService.AssociateFacesAsync(SystermId,new List<string>(){faceId}, userId);
                    }
                    else
                    {
                        //delete 
                        await _CollectionService.DeleteByFaceIdAsync(faceId,SystermId);
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
