using TMPro;
using UnityEngine;
using System.Reflection;
using System.IO;

public class VersionDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI version;

    void Start()
    {
        string buildTimestamp = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTimeUtc.ToString("yyyyMMddHHmmss");
        version.text = "Version: " + buildTimestamp + "-pre-alpha";
    }
}
