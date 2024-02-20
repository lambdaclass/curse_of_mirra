using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMaterialManager : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers = null;

    private List<RendererMaterialPair> renderer_material_pairs = new List<RendererMaterialPair>();

    private void Start()
    {
        init();
    }

    private void OnDestroy()
    {
        deinit();
    }

    public void init()
    {
        foreach(Renderer renderer in renderers)
        {
            renderer_material_pairs.Add(new RendererMaterialPair(renderer, renderer.sharedMaterial));
        }
    }

    public void deinit()
    {
        renderer_material_pairs.Clear();
    }

    public void setMaterial(Material material)
    {
        foreach(Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            renderer.sharedMaterial = material;
        }
    }

    public void resetMaterial()
    {
        foreach(Renderer renderer in renderers)
        {
            renderer_material_pairs.Add(new RendererMaterialPair(renderer, renderer.sharedMaterial));
            renderer.sharedMaterial = renderer_material_pairs.FirstOrDefault(x => x.renderer == renderer)?.material;
        }
    }
}

public class RendererMaterialPair
{
    public Renderer renderer = null;
    public Material material = null;

    public RendererMaterialPair(Renderer renderer, Material material)
    {
        this.renderer = renderer;
        this.material = material;
    }
}