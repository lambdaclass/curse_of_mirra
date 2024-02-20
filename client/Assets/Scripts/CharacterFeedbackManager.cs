using System;
using System.Linq;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField] Material transparentMaterial;

    private Material initialMaterial;

    void Awake()
    {
        initialMaterial = skinnedMeshRenderer.material;
    }
       public void ManageStateFeedbacks(
        Entity playerUpdate,
        CustomCharacter character
    )
    {
        if(playerUpdate.Player.Effects.Values.Any(effect => effect.Name == "invisible")){
            HandleInvisible(playerUpdate.Id, character);
        } else {
            skinnedMeshRenderer.material = initialMaterial;
            var characterCard = character.characterBase.GetComponentInChildren<UIFollowCamera>();
            characterCard.GetComponent<CanvasGroup>().alpha = 1;
            characterCard.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
    

    private void HandleInvisible(ulong id, CustomCharacter character){
        bool isClient = GameServerConnectionManager.Instance.playerId == id;
        float alpha = isClient ? 0.5f : 0;
        skinnedMeshRenderer.material = transparentMaterial;
        Color color = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);
        if(!isClient){
            var characterCard = character.characterBase.GetComponentInChildren<UIFollowCamera>();
            characterCard.GetComponent<CanvasGroup>().alpha = 0;
            characterCard.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }
}
