using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.EventSystems;

public class ApplicationStartFlow : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/TitleScreen");
    }

    [UnityTest]
    public IEnumerator TestGameStart()
    {
        GameObject enterButton = GameObject.Find("ButtonContainer");
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("TitleScreen"));

        yield return new WaitForSeconds(3f);
        yield return CoClickButton(enterButton);

        yield return new WaitForSeconds(1f);
        Debug.Log("check scene");
        sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("MainScreen"));
    }

    IEnumerator CoClickButton(GameObject go)
    {
        // simulate a button click  
        var pointer = new PointerEventData(EventSystem.current);

        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerClickHandler);
    }
}
