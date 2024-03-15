using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{
    public bool hasTransparentMaterial;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

    [MMCondition("hasTransparentMaterial", true)]
    [SerializeField]
    Material transparentMaterial;

    [SerializeField]
    List<GameObject> vfxList;
    private Material initialMaterial;

    void Awake()
    {
        initialMaterial = skinnedMeshRenderer?.material;
    }

    public void ManageStateFeedbacks(Entity playerUpdate, CustomCharacter character)
    {
        if (skinnedMeshRenderer != null && transparentMaterial != null)
        {
            if (playerUpdate.Player.Effects.Values.Any(effect => effect.Name == "invisible"))
            {
                if(skinnedMeshRenderer.material.color.a == 1){
                    HandleInvisible(playerUpdate.Id, character);
                }
            }
            else
            {
                skinnedMeshRenderer.material = initialMaterial;
                var canvasHolder = character.characterBase.CanvasHolder;
                canvasHolder.GetComponent<CanvasGroup>().alpha = 1;
                SetMeshes(true, character);
                vfxList.ForEach(el => el.SetActive(true));
                character.GetComponent<CharacterFeedbacks>().SetColorOverlayAlpha(1);
            }
        }
    }

    private void HandleInvisible(ulong id, CustomCharacter character)
    {
        bool isClient = GameServerConnectionManager.Instance.playerId == id;
        float alpha = isClient ? 0.5f : 0;
        skinnedMeshRenderer.material = transparentMaterial;
        Color color = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
        character.GetComponent<CharacterFeedbacks>().SetColorOverlayAlpha(alpha);

        if (!isClient)
        {
            var canvasHolder = character.characterBase.CanvasHolder;
            canvasHolder.GetComponent<CanvasGroup>().alpha = 0;
            SetMeshes(false, character);
            vfxList.ForEach(el => el.SetActive(false));
        }
    }

    // Will use this later when delay is implemented and improve code
    private void HandleCharacterCard(CustomCharacter character, bool visible)
    {
        var canvasHolder = character.characterBase.CanvasHolder;
        canvasHolder.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        character.characterBase.PlayerName.GetComponent<MeshRenderer>().enabled = visible;
    }

    private void SetMeshes(bool isActive, CustomCharacter character)
    {
        List<MeshRenderer> meshes = character
            .characterBase
            .CharacterCard
            .GetComponentsInChildren<MeshRenderer>()
            .ToList();
        meshes.ForEach(mesh => mesh.enabled = isActive);
    }
}
