using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStates : MonoBehaviour
{
    [SerializeField]
    List<StateInfo> states;

    [SerializeField]
    GameObject statesContainer;

    [SerializeField]
    GameObject StateItem;

    List<GameObject> iconStates = new List<GameObject>();

    public void DisplayStateIcon(string stateName, bool isActive)
    {
        GameObject item = null;
        if (isActive && !iconStates?.Find(el => el.name == stateName))
        {
            StateInfo state = GetStateById(stateName);
            item = Instantiate(StateItem, statesContainer.transform);
            item.name = stateName;
            item.GetComponent<Image>().sprite = state.image;
            item.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            iconStates.Add(item);
        }
        if (!isActive)
        {
            RemoveIconState(stateName);
        }
    }

    public void RemoveIconState(string stateName)
    {
        GameObject iconToRemove = iconStates.Find(el => el.name == stateName);
        iconStates.Remove(iconToRemove);
        Destroy(iconToRemove);
    }

    private StateInfo GetStateById(string stateName)
    {
        return states.Find(el => el.name == stateName);
    }
}
