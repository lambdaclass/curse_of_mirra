using UnityEngine;
using UnityEngine.UI;

public class SetSpriteToSkillInput : MonoBehaviour
{
    public void SetInitialSprite(Sprite newSprite)
    {
        GetComponent<Image>().sprite = newSprite;
    }
}
