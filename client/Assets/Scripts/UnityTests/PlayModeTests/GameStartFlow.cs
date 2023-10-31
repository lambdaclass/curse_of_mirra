using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.EventSystems;

public class GameStartFlow : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/Lobbies");
    }

    // Creates a lobby in the Lobby scene
    [UnityTest]
    public IEnumerator GameStart()
    {

        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return TestingUtils.CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));
        yield return new WaitForSeconds(2f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return TestingUtils.CoClickButton(launchGameButton);
        yield return new WaitForSeconds(2f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForFixedUpdate();

        GameObject selectH4ckButton = GameObject.Find("H4ck");
        Assert.That(selectH4ckButton.GetComponent<UICharacterItem>().selected, Is.EqualTo(false));
        yield return TestingUtils.CoClickButton(selectH4ckButton);
        yield return new WaitForSeconds(2f);
        Assert.That(selectH4ckButton.GetComponent<UICharacterItem>().selected, Is.EqualTo(true));
        yield return new WaitForSeconds(2f);

        GameObject confirmButton = GameObject.Find("ConfirmButton");
        yield return TestingUtils.CoClickButton(confirmButton);
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Battle"));
    }
}
