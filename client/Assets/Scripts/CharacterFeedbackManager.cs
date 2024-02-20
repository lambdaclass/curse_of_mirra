using System;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{

    public bool isUma;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    [MMCondition("isUma", true)]
    [SerializeField] Material transparentMaterial;

    private Material initialMaterial;

    void Awake()
    {
        initialMaterial = skinnedMeshRenderer?.material;
    }
       public void ManageStateFeedbacks(
        Entity playerUpdate,
        CustomCharacter character
    )
    {
        if(skinnedMeshRenderer != null && transparentMaterial != null){
            if(playerUpdate.Player.Effects.Values.Any(effect => effect.Name == "invisible")){
                HandleInvisible(playerUpdate.Id, character);
            } else {
                skinnedMeshRenderer.material = initialMaterial;
                var canvasHolder = character.characterBase.CanvasHolder;
                canvasHolder.GetComponent<CanvasGroup>().alpha = 1;
                character.characterBase.PlayerName.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        
    }
    

    private void HandleInvisible(ulong id, CustomCharacter character){
        bool isClient = GameServerConnectionManager.Instance.playerId == id;
        float alpha = isClient ? 0.5f : 0;
        skinnedMeshRenderer.material = transparentMaterial;
        Color color = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
        if(!isClient){
            var canvasHolder = character.characterBase.CanvasHolder;
            canvasHolder.GetComponent<CanvasGroup>().alpha = 0;
            character.characterBase.PlayerName.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // Will use this later when delay is implemented and improve code
    private void HandleCharacterCard(CustomCharacter character, bool visible){
        var canvasHolder = character.characterBase.CanvasHolder;
        canvasHolder.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        character.characterBase.PlayerName.GetComponent<MeshRenderer>().enabled = visible;
    }   
}
