﻿using Amazon.Rekognition.Model;

namespace UserRekongition.Services.IServices
{
    public interface ICollectionService
    {
        Task<bool> DeleteByUserIdAsync(string id, string systermId);
        Task<bool> IsCollectionExistAsync(string systermId);
        Task<bool> DeleteByFaceIdAsync(string faceId, string systermId);
        Task<bool> DisassociatedFaceAsync(string systermId, string faceId, string userId);
        Task<bool> CreateCollectionAsync(string systermId);
        Task<IndexFacesResponse> IndexFaceAsync(string systermId, string bucketName,string imageName, string key = null);
        Task<DetectFacesResponse> DetectFaceByFileAsync(IFormFile file);
        Task<bool> AssociateFacesAsync(string systermId, List<string> faceId, string key); 
        Task<bool> IsUserExistByUserIdAsync(string systermId, string userId);
        Task<bool> CreateNewUserAsync(string systermId, string userId);
        Task<SearchUsersResponse> SearchUserByFaceIdsAsync(string systermId, string faceId);
        Task<List<Face>> GetFacesAsync(string systermId);
    }
}
