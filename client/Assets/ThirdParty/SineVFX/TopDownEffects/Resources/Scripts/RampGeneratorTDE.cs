using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RampGeneratorTDE : MonoBehaviour
{

    public Gradient procedrualGradientRamp;
    public string pathForPNG = "/SineVFX/TopDownEffects/Recources/Textures/ForVFX/RampsGenerated/";

    public enum Mode {CreateAtStart, UpdateEveryFrame, BakeAndSaveAsTexture };
    public Renderer[] renderers;
    public Mode mode;
    

    private Texture2D rampTexture;
    private Texture2D tempTexture;
    private float width = 256;
    private float height = 64;

    void Start()
    {
        switch (mode)
        {
            case Mode.CreateAtStart:
                UpdateRampTexture();
                break;
            case Mode.UpdateEveryFrame:
                UpdateRampTexture();
                break;
            case Mode.BakeAndSaveAsTexture:
                break;
        }
    }

    void Update()
    {
        switch (mode)
        {
            case Mode.CreateAtStart:
                break;
            case Mode.UpdateEveryFrame:
                UpdateRampTexture();
                break;
            case Mode.BakeAndSaveAsTexture:
                break;
        }
    }

    // Generating a texture from gradient variable
    Texture2D GenerateTextureFromGradient(Gradient grad, float textureheight)
    {
        if (tempTexture == null)
        {
            tempTexture = new Texture2D((int)width, (int)textureheight);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < textureheight; y++)
            {
                Color col = grad.Evaluate(0 + (x / width));
                tempTexture.SetPixel(x, y, col);
            }
        }
        tempTexture.wrapMode = TextureWrapMode.Clamp;
        tempTexture.Apply();
        return tempTexture;
    }

    // Update procedural ramp textures and applying them to the shaders
    public void UpdateRampTexture()
    {
        rampTexture = GenerateTextureFromGradient(procedrualGradientRamp, height);
        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetTexture("_Ramp", rampTexture);
            }
        }
        
    }

    // Baking the gradient texture as PNG image
    public void BakeGradient()
    {
        rampTexture = GenerateTextureFromGradient(procedrualGradientRamp, 64);
        byte[] _bytes = rampTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + pathForPNG + "GeneratedRamp_" + Random.Range(0,99999).ToString() + ".png", _bytes);
    }
}
