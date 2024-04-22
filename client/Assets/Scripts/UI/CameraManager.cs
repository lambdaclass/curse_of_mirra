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
    CinemachineVirtualCamera mainCameraCM;

    [SerializeField]
    Image[] cameraToggles = new Image[4];

    [SerializeField]
    TextMeshProUGUI[] cameraTextHolders = new TextMeshProUGUI[4];

    private Vector3 defaultCameraRotation;
    private Vector3 topView = new Vector3(90, 0, 0);
    private float initialCameraDistance, variableCameraDistance;
    CinemachineFramingTransposer cinemachineFramingTransposer;
    
    void Awake()
    {        
        defaultCameraRotation = mainCamera.transform.rotation.eulerAngles;
        cinemachineFramingTransposer = mainCameraCM.GetCinemachineComponent<CinemachineFramingTransposer>();
        initialCameraDistance = cinemachineFramingTransposer.m_CameraDistance;
        SetDefaultSettings();
    }

    public void SetDefaultSettings()
    {
        SetCamera(defaultCameraRotation, initialCameraDistance, false);
        UpdateUIState(0);
    }
    public void SetZoomInPerspective(){
        SetCamera(defaultCameraRotation, initialCameraDistance - 10, false);
        UpdateUIState(1);
    }
    public void SetZoomOutPerspective(){
        SetCamera(defaultCameraRotation, initialCameraDistance + 10, false);
        UpdateUIState(2);
    }

    public void SetSentinelSettings()
    {
        SetCamera(topView, initialCameraDistance, true);       
        UpdateUIState(3); 
    }
    public void SetCamera(Vector3 cameraRotation, float cameraDistance, bool isOrthographic)
    {
        mainCameraCM.transform.rotation = Quaternion.Euler(cameraRotation);
        cinemachineFramingTransposer.m_CameraDistance = cameraDistance;
        mainCamera.orthographic = isOrthographic;
    }

    private void UpdateUIState(int index)
    {
        for(int i = 0; i < cameraToggles.Length; i++){
            bool isOn = (i == index);
            int alpha = isOn ? 255 : 100;
            cameraToggles[i].enabled = isOn;
            cameraTextHolders[i].color = new Color32(255, 255, 255, (byte)alpha);
        }
    }
}
