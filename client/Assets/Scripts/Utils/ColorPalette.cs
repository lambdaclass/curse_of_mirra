using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Custom/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public Color white = new Color32(255,255,255,255);
    public Color white25 = new Color32(255,255,255,63);
    public Color red50 = new Color32 (255,0,0,127);
}
