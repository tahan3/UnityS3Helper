using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.AWSS3
{
    public class S3AmazonEditorHelper : UnityEditor.Editor
    {
        [MenuItem("S3Helper/List bucket content")]
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

        [MenuItem("S3Helper/Refresh settings")]
        private static void Refresh()
        {
            EditorUtility.RequestScriptReload();
        }
        
        [MenuItem("S3Helper/Download bucket content")]
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
        
        [MenuItem("S3Helper/Edit settings")]
        private static void EditSettings()
        {
            Selection.activeObject = CreateSettings();
        }

        private static S3Settings CreateSettings()
        {
            S3Settings settings = S3Settings.Load();

            if (settings == null) {
                settings = CreateInstance<S3Settings>();
                
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    AssetDatabase.CreateFolder("Assets", "Resources");

                if (!AssetDatabase.IsValidFolder("Assets/Resources/S3Helper"))
                    AssetDatabase.CreateFolder("Assets/Resources", "S3Helper");
                
                AssetDatabase.CreateAsset(settings, "Assets/Resources/S3Helper/S3Settings.asset");
                settings = S3Settings.Load();
            }
            
            return settings;
        }
    }
}