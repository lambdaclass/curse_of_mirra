using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

public class GameCreationFlow : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/TitleScreen");
    }

    [UnityTest]
    public IEnumerator TestGameStart()
    // Goes from Title Screen to Main Screen by clicking the "Enter" button
    {
        GameObject enterButton = GameObject.Find("ButtonContainer");
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("TitleScreen"));
        Debug.Log("Enter Title screen - OK");

        yield return new WaitForSeconds(3f);

        ClickUI(enterButton);
        Debug.Log("Click on Play Now button - OK");

        yield return new WaitForSeconds(1f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MainScreen"));
        Debug.Log("Enter Main Screen - OK");

        // Goes from Main Screen to Lobbies screen by clicking on "Play Game"

        GameObject playGameButton = GameObject.Find("PlayGameButton");
        Assert.That(playGameButton, !Is.Null);

        Debug.Log("Play Game button is present - OK");

        ClickUI(playGameButton);
        Debug.Log("Click on Play Game button - OK");

        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
        Debug.Log("Ener Lobbies - OK");
    }
    public void ClickUI(GameObject uiElement)
    {
        uiElement.GetComponent<MMTouchButton>().ButtonReleased.Invoke();
    }
}
