using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine;

namespace Plugins.AWSS3
{
    public static class S3AmazonHelper
    {
        private const string BucketName = "bucketname";
        private const string Region = "region";
        private const string AccessKey = "accesskey";
        private const string SecretKey = "secretkey";
        private const string SpaceURL = "amazonaws.com";
        private static string SpaceEndpoint => $"https://{Region}.{SpaceURL}";

        private static readonly IAmazonS3 S3Client;
        
        static S3AmazonHelper()
        {
            AmazonS3Config config = new AmazonS3Config
            {
                ServiceURL = SpaceEndpoint,
                ForcePathStyle = true,
            };
            
            S3Client = new AmazonS3Client(AccessKey, SecretKey, config);
        }
        
        public static async Task<bool> ListBucketContentsAsync()
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = BucketName,
                    MaxKeys = 10,
                };

                ListObjectsV2Response response;

                do
                {
                    response = await S3Client.ListObjectsV2Async(request);

                    response.S3Objects
                        .ForEach(obj => Debug.Log($"{obj.Key,-35} {obj.LastModified.ToShortDateString(),10} {obj.Size,10}"));
                    
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Debug.Log($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return false;
            }
        }

        public static async Task<bool> DownloadAllFiles()
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = BucketName,
                    MaxKeys = 10,
                };

                ListObjectsV2Response response;

                do
                {
                    response = await S3Client.ListObjectsV2Async(request);
                    
                    foreach (var responseS3Object in response.S3Objects)
                    {
                        DownloadFile(responseS3Object.Key, Application.persistentDataPath + "/" + responseS3Object.Key);
                    }
                    
                    request.ContinuationToken = response.NextContinuationToken;
                }
                while (response.IsTruncated);
                
                Debug.Log($"Files location: {Application.persistentDataPath}");

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Debug.Log($"Error encountered on server. Message:'{ex.Message}' getting list of objects.");
                return false;
            }
        }

        public static async Task<bool> DownloadFile(string remoteFilePath, string localFilePath)
        {
            try
            {
                GetObjectRequest getRequest = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = remoteFilePath
                };
                
                using (GetObjectResponse response = await S3Client.GetObjectAsync(getRequest))
                {
                    await response.WriteResponseStreamToFileAsync(localFilePath, true, CancellationToken.None);
                }

                Debug.Log($"File downloaded successfully to {localFilePath}");
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Debug.Log($"Error downloading file: {e.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.Log($"Unknown error: {e.Message}");
                return false;
            }
        }
        
        private static Texture2D FileToTexture(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }

        public static async Task<string> GetRemoteTextFile(string remoteFilePath, string localFilePath)
        {
            if (!File.Exists(localFilePath))
            {
                await DownloadFile(remoteFilePath, localFilePath);
            }

            return await File.ReadAllTextAsync(localFilePath);
        }
        
        public static async Task<Texture2D> GetRemoteTexture(string remoteFilePath, string localFilePath)
        {
            if (!File.Exists(localFilePath))
            {
                await DownloadFile(remoteFilePath, localFilePath);
            }
            
            return FileToTexture(localFilePath);;
        }
    }
}
