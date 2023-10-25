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
    public IEnumerator BacktoLobbiesButton()
    {
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return CoClickButton(launchGameButton);
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));

        GameObject backButton = GameObject.Find("Back");
        yield return CoClickButton(backButton);
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobbies"));
    }

    [UnityTest]
    public IEnumerator MuteButton()
    {
        Debug.Log("start click");
        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(1f);

        Debug.Log("start click");
        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return CoClickButton(launchGameButton);
        yield return new WaitForSeconds(1f);

        // This test is set up as being muted by default.
        // This may seem wrong, but it's not. The IsMuted() method does exactly the opposite of what its name suggests.
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(false));
        GameObject muteButton = GameObject.Find("MuteButton");
        yield return CoClickButton(muteButton);

        yield return new WaitForSeconds(1f);
        var toggleAudioRef = muteButton.GetComponent<ToggleAudio>();
        // Verify icon changed
        Assert.That(toggleAudioRef.GetComponentInChildren<Image>().overrideSprite, Is.EqualTo(toggleAudioRef.unmutedSprite));
        // Verify sound manager is unmuted
        Assert.That(MMSoundManager.Instance.IsMuted(MMSoundManager.MMSoundManagerTracks.Master), Is.EqualTo(true));
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
