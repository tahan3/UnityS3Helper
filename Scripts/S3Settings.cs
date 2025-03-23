using UnityEngine;

namespace Plugins.AWSS3
{
    [CreateAssetMenu(fileName = "Assets/Resources/S3Helper/S3Settings", menuName = "S3Helper/S3Settings", order = 0)]
    public class S3Settings : ScriptableObject
    {
        private const string SETTING_RESOURCES_PATH = "S3Helper/S3Settings";
        
        public static S3Settings Load() => Resources.Load<S3Settings>(SETTING_RESOURCES_PATH);

        [Tooltip("Bucket name")]
        public string bucketName;

        [Tooltip("Region")]
        public string region;
        
        [Tooltip("Space access key")]
        public string accessKey;
        
        [Tooltip("Space secret key")]
        public string secretKey;
        
        [Tooltip("Space address")]
        public string spaceURL;
        
        public string SpaceEndpoint => $"https://{region}.{spaceURL}";
    }
}