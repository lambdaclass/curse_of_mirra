using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAnimations : MonoBehaviour
{
    public void SetAnimBool()
    {
        this.gameObject.SetActive(false);
        this.GetComponent<Animator>().SetBool("NewRound", false);
        print("Animation finished");
    }
}
