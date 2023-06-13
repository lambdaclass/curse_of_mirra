using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
    }

    public List<GameObject> GetAllChilds()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }
        return list;
    }
}
