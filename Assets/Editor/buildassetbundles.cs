using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        EnsureDirectory(assetBundleDirectory);

        #region windows
        string windowsPath = Path.Combine(assetBundleDirectory, "windows");
        EnsureDirectory(windowsPath);

        BuildPipeline.BuildAssetBundles(
            windowsPath,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);
        #endregion

        #region linux
        string linuxPath = Path.Combine(assetBundleDirectory, "linux");
        EnsureDirectory(linuxPath);

        BuildPipeline.BuildAssetBundles(
            linuxPath,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneLinux64);
        #endregion

        #region osx 
        string osxPath = Path.Combine(assetBundleDirectory, "osx");
        EnsureDirectory(osxPath);

        BuildPipeline.BuildAssetBundles(
            osxPath,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneOSX);
        #endregion

        // Adjust these paths per mod, too lazy to make it find assetbundles dynamically and stuff
        // Note: %locallow% probably doesn't exist on your machine (I added it to my system variables)
        CopyAssetbundleToModfolder(Path.Combine(assetBundleDirectory, "windows", "logvieweruiassets"),
             @"%locallow%\bitrich\Rail Route\mods\000_Logviewer\LogviewerUIAssets");
    }

    private static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
   
    private static void CopyAssetbundleToModfolder(string source, string target)
    {
        target = System.Environment.ExpandEnvironmentVariables(target);
        File.Copy(source, target, true);
    }
}