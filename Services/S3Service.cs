﻿using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using UserRekongition.Services.IServices;

namespace UserRekongition.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _service;
        private const long _maxFileSize = 15L * 1024 * 1024 * 1024; // 15 GB, có thể tùy chỉnh
        private const long _partSize = 500 * 1024 * 1024; // 500 MB - Part size lớn cho multipart upload
        private const long _divideSize = 500 * 1024 * 1024; // 500 MB - Part size lớn cho multipart upload


        public S3Service(IAmazonS3 service)
        {
            _service = service;
        }

        public async Task<bool> AddFileToS3Async(IFormFile file, string fileName, string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_service);

                // Sử dụng stream để upload
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    // Kiểm tra kích thước tệp và quyết định phương thức upload
                    if (file.Length < _divideSize)
                    {
                        // Upload file nhỏ hơn TransferUtility.MinimumPartSize (5MB mặc định)
                        await fileTransferUtility.UploadAsync(stream, bucketName, fileName);
                    }
                    else
                    {
                        // Sử dụng multipart upload cho tệp lớn hơn
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = stream,
                            Key = fileName,
                            BucketName = bucketName,
                            PartSize = _partSize, // Multipart upload với phần lớn hơn (500 MB)
                            StorageClass = S3StorageClass.Standard, // Có thể điều chỉnh storage class
                        };

                        await fileTransferUtility.UploadAsync(uploadRequest);
                    }
                }
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> IsExistBudget(string bucketName)
        {
            try
            {
                return await _service.DoesS3BucketExistAsync(bucketName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
