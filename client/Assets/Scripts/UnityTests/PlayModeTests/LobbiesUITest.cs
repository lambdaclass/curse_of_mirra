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
    public IEnumerator BacktoLobbiesButton()
    {
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        ClickUI(newLobbyButton);
        yield return new WaitForSeconds(1f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        ClickUI(launchGameButton);
        yield return new WaitForSeconds(1f);

        GameObject backButton = GameObject.Find("Back");
        ClickUI(backButton);
        yield return new WaitForSeconds(1f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
    }
    [UnityTest]
    public IEnumerator MuteButton()
    {
        Debug.Log("Entering MuteButton test");
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        ClickUI(newLobbyButton);
        yield return new WaitForSeconds(1f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        ClickUI(launchGameButton);
        yield return new WaitForSeconds(1f);

        GameObject muteButton = GameObject.Find("MuteButton");
        ClickUI(muteButton);

        yield return new WaitForSeconds(1f);
        var toggleAudioRef = muteButton.GetComponent<ToggleAudio>();
        Assert.That(toggleAudioRef.GetComponentInChildren<Image>().overrideSprite, Is.EqualTo(toggleAudioRef.unmutedSprite));
    }

    public void ClickUI(GameObject uiElement)
    {
        uiElement.GetComponent<MMTouchButton>().ButtonReleased.Invoke();
        Debug.Log("Clicking button");
    }
}
