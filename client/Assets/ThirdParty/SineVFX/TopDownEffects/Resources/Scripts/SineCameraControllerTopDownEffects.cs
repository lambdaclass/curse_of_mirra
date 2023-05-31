using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineCameraControllerTopDownEffects : MonoBehaviour
{

    public new Camera camera;
    public Transform basePivot;
    public Transform farPivot;
    public float scrollSpeed = 10f;
    public float rotationSpeed = 10f;
    public float rotationAmount = 2f;
    [Range(10f, 40f)]
    public float maximumAngle = 20f;

    private float closeFar = 0.5f;
    private float closeFarLerp = 0.5f;
    private Vector3 mouseAxisToVector;
    private float x;
    private float y;
    private Quaternion rotation;
    private bool rotationPossible = false;

    void Start()
    {
        rotation = gameObject.transform.localRotation;
        mouseAxisToVector = new Vector3(0f, 0f, 0f);
    }

    // Changing the rotation of the Main Camera in Update
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (closeFar < 1f)
            {
                closeFar += 0.1f;
            }
            if (closeFar > 1f)
            {
                closeFar = 1f;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (closeFar > 0f)
            {
                closeFar -= 0.1f;
            }
            if (closeFar < 0f)
            {
                closeFar = 0f;
            }
        }
        closeFarLerp = Mathf.Lerp(closeFarLerp, closeFar, Time.deltaTime * scrollSpeed);
        camera.transform.position = Vector3.Lerp(farPivot.position, basePivot.position, closeFarLerp);

        if (Input.GetMouseButton(0))
        {
            rotationPossible = true;
        }
        else
        {
            rotationPossible = false;
        }

        if (rotationPossible == true)
        {
            rotation = gameObject.transform.localRotation;
            x = rotation.eulerAngles.x + Input.GetAxis("Mouse Y") * rotationAmount;
            if (x > maximumAngle && x < 180)
            {
                x = maximumAngle;
            }
            if (x < 340f && x > 180f)
            {
                x = 340f;
            }
            y = rotation.eulerAngles.y + Input.GetAxis("Mouse X") * rotationAmount;
            mouseAxisToVector.Set(x, y, 0f);
            rotation.eulerAngles = mouseAxisToVector;
            gameObject.transform.localRotation = rotation;
        }
    }
}
