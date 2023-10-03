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

    [SerializeField]
    Image image;

    public void SetImage(int markCount)
    {
        if (markCount == 0)
        {
            gameObject.SetActive(false);
        }
        if (markCount == 1)
        {
            image.sprite = mark1;
        }
        if (markCount == 2)
        {
            image.sprite = mark2;
        }
        if (markCount == 3)
        {
            image.sprite = mark3;
        }
    }
}
