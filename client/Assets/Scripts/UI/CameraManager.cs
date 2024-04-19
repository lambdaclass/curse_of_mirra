using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] 
    Camera mainCamera;

    [SerializeField] 
    GameObject mainCameraCM;

    [SerializeField]
    List<GameObject> cameraToggles;

    private Vector3 defaultCameraRotation;
    private Vector3 topView = new Vector3(90, 0, 0);
    private float initialCameraDistance, variableCameraDistance;
    
    void Awake()
    {
        defaultCameraRotation = mainCamera.transform.rotation.eulerAngles;
        variableCameraDistance = mainCameraCM.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance;
        initialCameraDistance = variableCameraDistance;
        SetDefaultSettings();
    }

    public void SetDefaultSettings()
    {
        SetCamera(defaultCameraRotation, initialCameraDistance, false);
        UpdateUIState(cameraToggles[0]);
    }
    public void SetZoomInPerspective(){
        SetCamera(defaultCameraRotation, initialCameraDistance - 10, false);
        UpdateUIState(cameraToggles[1]);
    }
    public void SetZoomOutPerspective(){
        SetCamera(defaultCameraRotation, initialCameraDistance + 10, false);
        UpdateUIState(cameraToggles[2]);
    }

    public void SetSentinelSettings()
    {
        SetCamera(topView, initialCameraDistance, true);       
        UpdateUIState(cameraToggles[3]); 
    }
    public void SetCamera(Vector3 cameraRotation, float cameraDistance, bool orthographic)
    {
        mainCameraCM.transform.rotation = Quaternion.Euler(cameraRotation);
        variableCameraDistance = cameraDistance;
        mainCamera.orthographic = orthographic;
    }

    private void UpdateUIState(GameObject toggle)
    {
        foreach(GameObject cameraToggle in cameraToggles){
            bool isOn = (cameraToggle == toggle);
            cameraToggle.GetComponent<Image>().enabled = isOn;
            int alpha = isOn ? 255 : 100;
            cameraToggle.GetComponent<ToggleLabelHandler>().toggleLabel.color = new Color32(255, 255, 255, (byte)alpha);
        }
    }
}
