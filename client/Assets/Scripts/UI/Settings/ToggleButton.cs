using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI textState;

    // This is a quick fix until a refactor on this is done
    [Tooltip("Check this if the toogle is the console toggle")]
    public bool console;

    void Start()
    {
        if (GetComponent<MMTouchButton>())
        {
            GetComponent<MMTouchButton>().ReturnToInitialSpriteAutomatically = false;
        }
        if (transform.parent.GetComponent<MMTouchButton>())
        {
            transform.parent.GetComponent<MMTouchButton>().ReturnToInitialSpriteAutomatically =
                false;
        }
    }

    public void ToggleWithSiblingComponentBool(bool value)
    {
        ToggleUIState(value);
    }

    public void ToggleUIState(bool isOn)
    {
        GetComponent<Image>().enabled = isOn;
        int alpha = isOn ? 255 : 100;
        textState.color = new Color32(255, 255, 255, (byte)alpha);
    }
}
