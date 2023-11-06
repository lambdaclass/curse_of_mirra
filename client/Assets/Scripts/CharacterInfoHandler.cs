using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoHandler : MonoBehaviour
{
    public Camera playerCamera;

    void Update()
    {
        transform.LookAt(
            transform.position + playerCamera.transform.rotation * Vector3.back,
            playerCamera.transform.rotation * Vector3.up
        );
    }
}
