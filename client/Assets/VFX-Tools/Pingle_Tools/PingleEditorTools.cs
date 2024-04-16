#if UNITY_EDITOR
using UnityEditor;

public class PingleEditorTools
{
    [MenuItem("Tools/Toggle VFX_DEBUG")]
    public static void Toggle_VFX_DEBUG()
    {
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (defines.Contains("VFX_DEBUG"))
        {
            defines = defines.Replace(";VFX_DEBUG", "");
        }
        else
        {
            defines += ";VFX_DEBUG";
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        Menu.SetChecked("Tools/Toggle VFX_DEBUG", defines.Contains("VFX_DEBUG"));
    }
}
#endif