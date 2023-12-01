using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public IEnumerator ApplicationStart()
    {
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("TitleScreen"));

        yield return new WaitForSeconds(3f);
        yield return TestingUtils.ForceClick("ButtonContainer");

        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MainScreen"));

        yield return TestingUtils.ForceClick("PlayGameButton");
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
    }
}
