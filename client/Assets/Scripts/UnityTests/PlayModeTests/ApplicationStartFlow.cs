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
        yield return TestingUtils.CoClickButton(enterButton);

        yield return new WaitForSeconds(1f);
        Debug.Log("check scene");
        sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("MainScreen"));
    }
}
