using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Logviewer.IO
{
    public static class Utils
    {
        public const string SaveFolderName = "logs";

        /// <summary>
        /// Returns the assembly directory path
        /// </summary>
        public static string GetAssemblyDirectory() =>
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Returns the save directory path
        /// </summary>
        public static string GetSavesDirectory() =>
            Path.Combine(GetAssemblyDirectory(), SaveFolderName);

        /// <summary>
        /// Create a directory if it does not already exist
        /// </summary>
        /// <returns>true if the directory was created or already exists,
        /// false if a problem was encountered</returns>
        public static bool TryCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (IOException e) // these are the most likely to occur
            {
                Debug.LogError($"[Logviewer] IOException occurred when creating the {path} directory. " +
                    $"Message: {e.Message}, HResult: {e.HResult}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Logviewer] {e.GetType()} occurred when creating the {path} directory. " +
                    $"Message: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Write <paramref name="contents"/> to a file located at <paramref name="path"/>
        /// <returns>Whether or not writing was successful</returns>
        public static bool TrySaveToFile(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
                return true;
            }
            catch (IOException e) // these are the most likely to occur
            {
                Debug.LogError($"[Logviewer] IOException occurred when creating {path}. " +
                    $"Message: {e.Message}, HResult: {e.HResult}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Logviewer] {e.GetType()} occurred when creating {path}. " +
                    $"Message: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Read <paramref name="contents"/> from a file located at <paramref name="path"/>
        /// </summary>
        /// <returns>Whether or not reading was successful</returns>
        public static bool TryLoadFromFile(string path, out string contents)
        {
            contents = "{}"; // empty dictionary to avoid deserialization problems

            try
            {
                contents = File.ReadAllText(path);
                return true;
            }
            catch (IOException e) // these are the most likely to occur
            {
                Debug.LogError($"[Logviewer] IOException occurred when reading {path}. " +
                    $"Message: {e.Message}, HResult: {e.HResult}");
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"[Logviewer] {e.GetType()} occurred when reading {path}. " +
                    $"Message: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads the assetbundle with filename <paramref name="name"/>
        /// from the directory this assembly is in
        /// </summary>
        /// <returns>The assetbundle, or null if loading failed</returns>
        public static AssetBundle LoadAssetBundle(string name)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            string assetBundlePath = Path.Combine(Path.GetDirectoryName(assembly), name);

            return AssetBundle.LoadFromFile(assetBundlePath);
        }
    }
}
