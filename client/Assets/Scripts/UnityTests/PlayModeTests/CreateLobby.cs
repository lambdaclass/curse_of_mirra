using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

public class LobbyCreationFlow : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/Lobbies");
    }

    [UnityTest]
    public IEnumerator LobbyCreation()
    // Creates a lobby in the Lobby scene
    {
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        ClickUI(newLobbyButton);
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));
        yield return new WaitForSeconds(1f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        ClickUI(launchGameButton);
        Debug.Log(launchGameButton);
        yield return new WaitForSeconds(3f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForFixedUpdate();

    }
    public void ClickUI(GameObject uiElement)
    {
        uiElement.GetComponent<MMTouchButton>().ButtonReleased.Invoke();
    }
}
