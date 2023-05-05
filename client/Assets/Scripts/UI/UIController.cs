using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject joystick;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideJoystick()
    {
        joystick.GetComponent<Image>().enabled = false;
    }
    public void ShowJoystick()
    {
        joystick.GetComponent<Image>().enabled = true;
    }
}
