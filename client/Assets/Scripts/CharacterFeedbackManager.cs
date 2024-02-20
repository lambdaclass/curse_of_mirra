using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFeedbackManager : MonoBehaviour
{
    public void ToggleHealthBar(GameObject player, Entity playerUpdate)
    {
        var healthBarFront = player
            .GetComponent<CustomCharacter>()
            .GetComponent<MMHealthBar>()
            .TargetProgressBar
            .ForegroundBar
            .GetComponent<Image>();
       healthBarFront.color = Utils.healthBarRed;
    }

    public void ManageStateFeedbacks(
        GameObject player,
        Entity playerUpdate,
        CustomCharacter character
    )
    {
        this.ToggleHealthBar(player, playerUpdate);
    }
}
