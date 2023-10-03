using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateManagerUI : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> states;

    public void ToggleState(string name, bool isActive)
    {
        GetState(name).SetActive(isActive);
    }

    public GameObject GetState(string name)
    {
        return states.Find(state => state.name == name);
    }

    public void ClearAllStates()
    {
        states.ForEach(el => el.SetActive(false));
    }
}
