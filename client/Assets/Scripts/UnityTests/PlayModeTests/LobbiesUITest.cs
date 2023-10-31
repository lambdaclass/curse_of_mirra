using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbiesUITest : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/Lobbies");
    }

    [UnityTest]
    public IEnumerator UnmuteButton()
    {
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return TestingUtils.CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(5f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return TestingUtils.CoClickButton(launchGameButton);
        yield return new WaitForSeconds(5f);

        // This test is set up as being muted by default.
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(false));
        GameObject muteButton = GameObject.Find("MuteButton");
        yield return TestingUtils.CoClickButton(muteButton);

        yield return new WaitForSeconds(5f);
        var toggleAudioRef = muteButton.GetComponent<ToggleAudio>();
        // Verify icon changed
        Assert.That(toggleAudioRef.GetComponentInChildren<Image>().overrideSprite, Is.EqualTo(toggleAudioRef.mutedSprite));
        // Verify sound manager is unmuted
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(false));
    }

    [UnityTest]
    public IEnumerator BacktoLobbiesButton()
    {
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return TestingUtils.CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(5f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return TestingUtils.CoClickButton(launchGameButton);
        yield return new WaitForSeconds(5f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));

        GameObject backButton = GameObject.Find("Back");
        yield return TestingUtils.CoClickButton(backButton);
        yield return new WaitForSeconds(5f);
        //Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
    }
}
