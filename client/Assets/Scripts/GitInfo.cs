#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

class GitInfo
{
    private static string GIT_HASH =  null;

    public static string GetGitHash()
    {
        if (GIT_HASH != null)
        {
            return GIT_HASH;
        }

        #if UNITY_EDITOR
            GIT_HASH = GetGitHashInOS();
        #else
            var gitHash = UnityEngine.Resources.Load<TextAsset>("GitHash");
            GIT_HASH = gitHash.text;
        #endif

        return GIT_HASH;
    }

    public static string GetGitHashInOS() {
        var procStartInfo = new System.Diagnostics.ProcessStartInfo("bash", "-c \"git rev-parse --short=8 HEAD\"");
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;

        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        proc.WaitForExit(5000);
        return proc.StandardOutput.ReadToEnd();
    }
}

#if UNITY_EDITOR
    class GitBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            string filepath = "Assets/Resources/GitHash.asset";

            AssetDatabase.DeleteAsset(filepath);
            var text = new TextAsset(GitInfo.GetGitHashInOS());
            AssetDatabase.CreateAsset(text, filepath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
#endif
