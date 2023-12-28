using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField]
    GameObject line;
    float lineCounter = 21f;

    void Start()
    {
        StartCoroutine(InsertLine());
    }

    IEnumerator InsertLine()
    {
        for (int i = 0; i < lineCounter; i++)
        {
            yield return new WaitForSeconds(.5f);
            Instantiate(line, this.transform);
        }
    }
}
