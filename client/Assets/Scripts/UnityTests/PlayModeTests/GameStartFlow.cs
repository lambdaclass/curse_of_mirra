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
        yield return TestingUtils.ForceClick("NewLobbyButton");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));
        yield return new WaitForSeconds(2f);

        yield return TestingUtils.ForceClick("LaunchGameButton");
        yield return new WaitForSeconds(2f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForFixedUpdate();

        UICharacterItem H4ckUICharacterItem = GameObject.Find("H4ck").GetComponent<UICharacterItem>();
        Assert.That(H4ckUICharacterItem.selected, Is.EqualTo(false));
        yield return TestingUtils.ForceClick("H4ck");
        yield return new WaitForSeconds(2f);
        Assert.That(H4ckUICharacterItem.selected, Is.EqualTo(true));
        yield return new WaitForSeconds(2f);

        yield return TestingUtils.ForceClick("ConfirmButton");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Battle"));
    }
}
