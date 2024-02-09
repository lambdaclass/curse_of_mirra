using UnityEngine;

[CreateAssetMenu(fileName = "PlacementHolder", menuName = "ScriptableObject/PlacementHolder")]
public class PlacementHolder : ScriptableObject
{
    [SerializeField]
    public string place_name = null;
}
