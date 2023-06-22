using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] public Text playerText;
    public int id;

    public int GetId()
    {
        return id;
    }

    public void SetId(int id)
    {
        this.id = id;
    }
}
