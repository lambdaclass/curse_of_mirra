using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    [SerializeField]
    GameObject useItemEffect;

    public void ExecuteFeedback(bool state)
    {
        useItemEffect.SetActive(state);
    }
}
