using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RampGeneratorTDE))]
public class CustomRampGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RampGeneratorTDE rampscript = (RampGeneratorTDE)target;

        if (rampscript.mode == RampGeneratorTDE.Mode.BakeAndSaveAsTexture)
        {
            if (GUILayout.Button("Create PNG Gradient Texture"))
            {
                rampscript.BakeGradient();
                AssetDatabase.Refresh();
            }
        }               
    }
}
