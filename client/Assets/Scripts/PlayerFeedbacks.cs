using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    [SerializeField]
    CustomInputManager InputManager;

    public void PlayDeathFeedback(Character player)
    {
        if (player.CharacterModel.activeSelf == true)
        {
            player.GetComponent<Health>().DeathMMFeedbacks.PlayFeedbacks();
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

    public void ChangePlayerTextureOnDamage(
        GameObject player,
        float clientHealth,
        float playerHealth
    )
    {
        // player.GetComponentInChildren<OverlayEffect>().enabled = true;

        if (clientHealth != playerHealth)
        {
            if (playerHealth < clientHealth)
            {
                player.GetComponentInChildren<OverlayEffect>().SetShader("Damage");
                player.GetComponentInChildren<OverlayEffect>().enabled = true;
            }
            if (playerHealth > clientHealth)
            {
                GameObject healFeedback = player
                    .GetComponentInChildren<FeedbackContainer>()
                    .GetFeedback("HealFeedback");

                healFeedback.GetComponent<MMF_Player>().PlayFeedbacks();
                player.GetComponentInChildren<OverlayEffect>().enabled = false;
            }
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

    public void ExecuteH4ckDisarmFeedback(bool disarmed)
    {
        InputManager.ActivateDisarmEffect(disarmed);
    }

    public void SetActiveFeedback(GameObject player, string feedbackName, bool value)
    {
        player
            .GetComponentInChildren<FeedbackContainer>()
            .SetActiveStateFeedback(feedbackName, true);
    }

    public void ClearAllFeedbacks(GameObject player)
    {
        player
            .GetComponentInChildren<FeedbackContainer>()
            .GetFeedbackStateList()
            .ForEach(el => el.SetActive(false));
    }
}
