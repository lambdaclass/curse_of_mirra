using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    // TODO: We need a way to unificate the models materials structure.
    [Tooltip("Dog model texture")]
    public Texture2D defaultPlayerTexture;
    public Material defaultH4ckTexture;

    Color32 damageColor = new Color32(219, 38, 38, 1);

    public void PlayDeathFeedback(GameObject player, Health healthComponent)
    {
        if (healthComponent.CurrentHealth <= 0 && player.GetComponent<Character>().CharacterModel.activeSelf == true)
        {
            healthComponent.DeathMMFeedbacks.PlayFeedbacks();
        }
    }

    public void DisplayDamageRecieved(GameObject player, Health healthComponent, float playerHealth, ulong id)
    {
        if (healthComponent.CurrentHealth != playerHealth &&
        SocketConnectionManager.Instance.playerId == id)
        {
            healthComponent.Damage(0.001f, this.gameObject, 0, 0, Vector3.up);
        }
    }

    public void ChangePlayerTextureOnDamage(GameObject player, float auxHealth, float playerHealth)
    {
        Transform characterModel = player.GetComponent<Character>().CharacterModel.transform;

        if (auxHealth != playerHealth)
        {
            if (characterModel.GetChild(0).GetComponent<Renderer>())
            {
                GetModelMaterial(characterModel, 0).mainTexture = Texture2D.redTexture;
            }
            if (characterModel.name == "H4ck")
            {
                GetModelMaterial(characterModel, 6).color = damageColor;
            }
        }
        else
        {
            // Set back to default
            if (GetModelMaterial(characterModel, 0).mainTexture == Texture2D.redTexture)
            {
                StartCoroutine(WaitToChangeTexture(characterModel));
            }

            if (characterModel.name == "H4ck" && GetModelMaterial(characterModel, 6).color == damageColor)
            {
                StartCoroutine(WaitToChangeTextureH4ck(characterModel));
            }

        }
    }

    IEnumerator WaitToChangeTexture(Transform modelTransform)
    {
        yield return new WaitForSeconds(0.3f);
        GetModelMaterial(modelTransform, 0).mainTexture = defaultPlayerTexture;
    }

    IEnumerator WaitToChangeTextureH4ck(Transform modelTransform)
    {
        yield return new WaitForSeconds(0.3f);
        print(defaultH4ckTexture.color);
        GetModelMaterial(modelTransform, 6).color = defaultH4ckTexture.color;
    }

    // I need this because the models we have for now are structured differently 
    private Material GetModelMaterial(Transform modelTransform, int childIndex)
    {
        return modelTransform.GetChild(childIndex).GetComponent<Renderer>().material;
    }
}
