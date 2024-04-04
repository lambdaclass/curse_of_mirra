using UnityEngine;

public class HitBoxesHandler : MonoBehaviour
{
    [SerializeField]
    bool isActive;
    private bool cameraDefault = true;
    private Camera mainCamera;
    private GameObject mainCameraCM;
    private Vector3 defaultCameraRotation;
    private Vector3 topView = new Vector3(90, 0, 0);

    void Awake()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        mainCameraCM = GameObject.Find("CM vcam1");
        defaultCameraRotation = mainCamera.transform.rotation.eulerAngles;
        SetMatrixModeSettings();
        SetCameraSettings();
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

    public void ToggleCamera()
    {
        cameraDefault = !cameraDefault;
        SetCameraSettings();
    }

    void SetCameraSettings()
    {
        if (cameraDefault)
        {
            mainCameraCM.transform.rotation = Quaternion.Euler(defaultCameraRotation);
            mainCamera.orthographic = false;
        }
        else
        {
            mainCameraCM.transform.rotation = Quaternion.Euler(topView);
            mainCamera.orthographic = true;
        }
        GetComponent<ToggleButton>().ToggleCamera(cameraDefault);
    }
}
