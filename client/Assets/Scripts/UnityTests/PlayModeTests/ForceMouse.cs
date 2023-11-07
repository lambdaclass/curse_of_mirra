using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MoreMountains.Tools;

public class ForceMouse : InputTestFixture
{
    Mouse mouse;
    GameObject button;
    GameObject UIMouse;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/TestScreen");
        mouse = InputSystem.AddDevice<Mouse>();
    }

    [UnityTest]
    public IEnumerator ForceClick()
    {
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("TestScreen"));
        button = GameObject.Find("Button");
        UIMouse = GameObject.Find("Mouse");
        // Here is where the button can't be clicked
        button.GetComponent<MMTouchButton>().MouseMode = false;
        ClickUI(button, UIMouse);
        yield return null;
    }

    public void ClickUI(GameObject uiElement, GameObject UIMouse)
    {
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
        Set(mouse.position, screenPos);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePos);

        UIMouse.transform.position = worldPosition;
    }
}
