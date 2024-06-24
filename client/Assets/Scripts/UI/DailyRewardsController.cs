using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class DailyRewardsController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClaimDailyReward()
    {

        Debug.Log("Entra a ClaimDailyReward()");

        StartCoroutine(
            ServerUtils.ClaimDailyReward(
                response =>
                {
                    Debug.Log("Response: " + response);
                },
                error =>
                {
                    Errors.Instance.HandleNetworkError("Error", error);
                }
            )
        );
        this.GetComponent<MMLoadScene>().LoadScene();
    }
}
