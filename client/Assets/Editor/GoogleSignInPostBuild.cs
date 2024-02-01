using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class GoogleSignInPostBuild
{
#if UNITY_IOS
    [PostProcessBuild(999)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        var proj = new PBXProject();
        proj.ReadFromFile(projPath);
        var targetGuid = proj.GetUnityMainTargetGuid();
        proj.AddFileToBuild(
            targetGuid,
            proj.AddFile("Data/Raw/GoogleService-Info.plist", "GoogleService-Info.plist")
        );
        proj.WriteToFile(projPath);
    }
#endif
}
