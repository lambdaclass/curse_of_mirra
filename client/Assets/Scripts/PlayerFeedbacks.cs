using System.Collections;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    [SerializeField]
    CustomInputManager InputManager;

    public void PlayDeathFeedback(GameObject player, Health healthComponent)
    {
        if (
            healthComponent.CurrentHealth <= 0
            && player.GetComponent<Character>().CharacterModel.activeSelf == true
        )
        {
            healthComponent.DeathMMFeedbacks.PlayFeedbacks();
        }
    }

    public void DisplayDamageRecieved(
        GameObject player,
        Health healthComponent,
        float playerHealth,
        ulong id
    )
    {
        if (
            healthComponent.CurrentHealth != playerHealth
            && SocketConnectionManager.Instance.playerId == id
        )
        {
            healthComponent.Damage(0.001f, this.gameObject, 0, 0, Vector3.up);
        }
    }

    public void ChangePlayerTextureOnDamage(GameObject player, float auxHealth, float playerHealth)
    {
        if (auxHealth != playerHealth)
        {
            player.GetComponentInChildren<OverlayEffect>().enabled = true;
        }
        else
        {
            if (player.GetComponentInChildren<OverlayEffect>().enabled)
            {
                StartCoroutine(WaitToRemoveShader(player));
            }
        }
    }

    IEnumerator WaitToRemoveShader(GameObject player)
    {
        yield return new WaitForSeconds(0.2f);
        player.GetComponentInChildren<OverlayEffect>().enabled = false;
    }

    public void ExecuteH4ckDisarmFeedback(ulong id, bool disarmed)
    {
        InputManager.ActivateDisarmEffect(disarmed);
        //Add Icon state
        GetComponent<PlayerStates>()
            .DisplayStateIcon(id, "Disarmed", disarmed);

        // Remove icon state
        if (!disarmed)
        {
            GetComponent<PlayerStates>().RemoveIconState("Disarmed");
        }
    }

    public void SetActivePoisonedFeedback(ulong id, GameObject player, bool active)
    {
        player.transform.Find("Poison").GetComponent<ParticleSystem>().gameObject.SetActive(active);
        //Add Icon state
        GetComponent<PlayerStates>()
            .DisplayStateIcon(
                id,
                "Poisoned",
                player.transform.Find("Poison").GetComponent<ParticleSystem>().gameObject.activeSelf
            );

        if (!active)
        {
            // Remove icon state
            GetComponent<PlayerStates>()
                .RemoveIconState("Poisoned");
        }
    }
}
