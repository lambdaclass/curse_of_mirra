using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.UI;

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
        yield return SetupLocalhostAsServer();
        yield return TestingUtils.ForceClick("NewLobbyButton");
        yield return new WaitForSeconds(2f);

        yield return TestingUtils.ForceClick("LaunchGameButton");
        yield return new WaitForSeconds(2f);

        // This test is set up as being muted by default.
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(false));
        yield return TestingUtils.ForceClick("MuteButton");

        yield return new WaitForSeconds(2f);
        var toggleAudioRef = GameObject.Find("MuteButton").GetComponent<ToggleAudio>();
        // Verify icon changed
        Assert.That(toggleAudioRef.GetComponentInChildren<Image>().overrideSprite, Is.EqualTo(toggleAudioRef.mutedSprite));
        // Verify sound manager is unmuted
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(false));
    }

    [UnityTest]
    public IEnumerator BackToLobbiesButton()
    {
        yield return SetupLocalhostAsServer();
        yield return TestingUtils.ForceClick("NewLobbyButton");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));

        yield return TestingUtils.ForceClick("LaunchGameButton");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));

        yield return TestingUtils.ForceClick("Back");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
    }

    IEnumerator SetupLocalhostAsServer() {
        yield return TestingUtils.ForceClick("ServerNameContainer");
        yield return new WaitForSeconds(.1f);
        yield return TestingUtils.ForceClick("LocalHost");
        yield return new WaitForSeconds(.1f);
    }
}
