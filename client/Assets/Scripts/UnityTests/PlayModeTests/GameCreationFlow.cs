using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameCreationFlow : InputTestFixture
{
    Mouse mouse;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/TitleScreen");
        mouse = InputSystem.AddDevice<Mouse>();
    }

    [UnityTest]
    public IEnumerator TestGameStart()
    {
        GameObject enterButton = GameObject.Find("Enter");
        Assert.That(enterButton, Is.Not.Null);
        yield return null;
    }

    /* public void ClickUI(GameObject uiElement)
    {
        Camera camera = GameObject.Find("Camera").GetComponent<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(uiElement.transform.position);
        Set(mouse.position, screenPos);
        Click(mouse.leftButton);
    } */
}
