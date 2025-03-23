using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.AWSS3
{
    public static class S3AmazonEditorHelper
    {
        [MenuItem("AWS.S3/List bucket content")]
        private static async void ListBucketContent()
        {
            try
            {
                await S3AmazonHelper.ListBucketContentsAsync();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        
        [MenuItem("AWS.S3/Download bucket content")]
        private static async void DownloadBucketContent()
        {
            try
            {
                await S3AmazonHelper.DownloadAllFiles();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}