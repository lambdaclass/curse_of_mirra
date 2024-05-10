using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSettingsBlock", menuName = "ScriptableObject/MaterialSettingsBlock")]
public class MaterialSettingsBlock : ScriptableObject
{
    [SerializeField] public MaterialSettingsKey key = null;
    [SerializeField] public float apply_duration = 0.3f;
    [SerializeField] public string controll_property = null;
    [SerializeField] public MaterialFloatValue[] float_values = null;
    [SerializeField] public MaterialColorValue[] color_values = null;

    public void applyToMaterial( Material mat )
    {
        foreach( MaterialFloatValue value in float_values )
            mat.SetFloat( value.name, value.value );

        foreach( MaterialColorValue value in color_values )
            mat.SetColor( value.name, value.value );
    }
}

[Serializable]
public class MaterialFloatValue
{
    [SerializeField] public string name = null;
    [SerializeField] public float value = 0.0f;
}

[Serializable]
public class MaterialColorValue
{
    [SerializeField] public string name = null;
    [SerializeField, ColorUsageAttribute(true, true)] public Color value = Color.white;
}