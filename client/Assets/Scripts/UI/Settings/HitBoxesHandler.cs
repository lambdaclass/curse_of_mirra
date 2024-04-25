using UnityEngine;

public class HitBoxesHandler : MonoBehaviour
{
    [SerializeField]
    bool isActive;
    void Awake()
    {
        SetMatrixModeSettings();
    }

    public void ToggleMatrixMode()
    {
        isActive = !isActive;
        SetMatrixModeSettings();
    }

    void SetMatrixModeSettings()
    {
        GameObject[] hitboxes = GameObject.FindGameObjectsWithTag("Hitbox");
        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.transform.GetChild(0).gameObject.SetActive(isActive);
        }
        GetComponent<ToggleButton>().ToggleWithSiblingComponentBool(isActive);
    }
}
