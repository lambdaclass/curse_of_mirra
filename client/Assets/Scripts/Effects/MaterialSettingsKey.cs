using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSettingsKey", menuName = "ScriptableObject/MaterialSettingsKey")]
public class MaterialSettingsKey : ScriptableObject
{
    [SerializeField] public string description = null;
}
