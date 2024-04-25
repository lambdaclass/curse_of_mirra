using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterFeedbackManager : MonoBehaviour
{
    public bool hasTransparentMaterial;

    [SerializeField]
    GameObject feedbacksContainer;

    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;

    [MMCondition("hasTransparentMaterial", true)]
    [SerializeField]
    Material transparentMaterial;

    [SerializeField]
    List<GameObject> vfxList;
    private Material initialMaterial;

    private Dictionary<int, GameObject> InstantiateItems = new Dictionary<int, GameObject>();

    void Awake()
    {
        initialMaterial = skinnedMeshRenderer?.material;
    }

    public void ManageStateFeedbacks(Entity playerUpdate, CustomCharacter character)
    {
        if (playerUpdate.Player.Effects.Count > 0)
        {
            CharacterFeedbacks feedback = character.GetComponent<CharacterFeedbacks>(); // maybe cache this? We can optimize this later.
            for (int i = 0; i < playerUpdate.Player.Effects.Count; i++)
            {
                var effect = playerUpdate.Player.Effects[i];
                var name = effect.Name;
                var duration = effect.DurationMs / 1000;
                var item = feedback.SelectGO(name);
                if (!InstantiateItems.ContainsKey(i) && item != null)
                {
                    var vfx = Instantiate(item, feedbacksContainer.transform);
                    vfx.name = name + "ID " + i;
                    InstantiateItems.Add(i, vfx);

                    vfx.GetComponent<PinnedEffectsController>()
                        ?.Setup(character.GetComponent<PinnedEffectsManager>());
                    StartCoroutine(
                        character
                            .GetComponent<CharacterMaterialManager>()
                            .ResetEffects(
                                duration,
                                vfx,
                                vfx.GetComponent<PinnedEffectsController>(),
                                InstantiateItems,
                                i
                            )
                    );
                }
            }
            ;
        }

        // Refacor this to a single metho to handle effects.

        if (skinnedMeshRenderer != null && transparentMaterial != null)
        {
            if (hasEffect(playerUpdate.Player.Effects, "invisible"))
            {
                if (skinnedMeshRenderer.sharedMaterial.HasProperty("_ISTRANSPARENT"))
                {
                    HandleInvisible(playerUpdate.Id, character);
                }
            }
            else
            {
                if (skinnedMeshRenderer.sharedMaterial.HasProperty("_ISTRANSPARENT"))
                {
                    if (skinnedMeshRenderer.sharedMaterial.GetInt("_ISTRANSPARENT") == 1)
                    {
                        skinnedMeshRenderer.material = initialMaterial;
                        var canvasHolder = character.characterBase.CanvasHolder;
                        canvasHolder.GetComponent<CanvasGroup>().alpha = 1;
                        SetMeshes(true, character);
                        vfxList.ForEach(el => el.SetActive(true));
                        feedbacksContainer.SetActive(true);
                        character.characterBase.characterShadow.SetActive(true);
                        character.GetComponent<CharacterFeedbacks>().SetColorOverlayAlpha(1);
                        skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
                    }
                }
            }
        }
    }

    private void HandleInvisible(ulong id, CustomCharacter character)
    {
        bool isClient = GameServerConnectionManager.Instance.playerId == id;
        float alpha = isClient ? 0.5f : 0;
        skinnedMeshRenderer.material = transparentMaterial;
        skinnedMeshRenderer.sharedMaterial.SetFloat("_AlphaValue", alpha);
        character.GetComponent<CharacterFeedbacks>().SetColorOverlayAlpha(alpha);

        if (!isClient)
        {
            var canvasHolder = character.characterBase.CanvasHolder;
            canvasHolder.GetComponent<CanvasGroup>().alpha = 0;
            SetMeshes(false, character);
            vfxList.ForEach(el => el.SetActive(false));
            feedbacksContainer.SetActive(false);
            character.characterBase.characterShadow.SetActive(false);
            skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
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
    }

    private bool hasEffect(ICollection<Effect> effects, string effectName)
    {
        foreach (var effect in effects)
        {
            if (effect.Name == effectName)
            {
                return true;
            }
        }
        return false;
    }

    public void HandlePickUpItemFeedback(Entity playerUpdate, CharacterFeedbacks characterFeedbacks)
    {
        if (playerUpdate.Player.Inventory != null && !characterFeedbacks.DidPickUp())
        {
            characterFeedbacks.ExecutePickUpItemFeedback(true);
        }
        else if (playerUpdate.Player.Inventory == null && characterFeedbacks.DidPickUp())
        {
            characterFeedbacks.ExecutePickUpItemFeedback(false);
        }
    }
}
