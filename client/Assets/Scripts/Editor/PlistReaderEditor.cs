#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PlistReaderEditor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        try
        {
            var filePath = "Assets/StreamingAssets/REVERSE_CLIENT.txt";
            string REVERSE_CLIENT = PlayerSettings.iOS.iOSUrlSchemes[0];
            System.IO.File.WriteAllText(filePath, REVERSE_CLIENT);
            Debug.Log("MyCustomBuildProcessor.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
#endif