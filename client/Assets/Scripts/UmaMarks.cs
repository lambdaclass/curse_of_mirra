using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UmaMarks : MonoBehaviour
{
    [SerializeField]
    Sprite mark1;

    [SerializeField]
    Sprite mark2;

    [SerializeField]
    Sprite mark3;

    public void SetImage(int markCount)
    {
        if (markCount == 1)
        {
            GetComponent<Image>().sprite = mark1;
        }
        if (markCount == 2)
        {
            GetComponent<Image>().sprite = mark2;
        }
        if (markCount == 3)
        {
            GetComponent<Image>().sprite = mark3;
        }
    }
}
