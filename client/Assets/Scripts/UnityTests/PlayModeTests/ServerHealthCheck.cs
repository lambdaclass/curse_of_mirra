using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.EventSystems;

public class ServerHealthCheck : InputTestFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        SceneManager.LoadScene("Scenes/Lobbies");
    }

    // Creates a lobby in the Lobby scene
    [UnityTest]
    public IEnumerator ServerIsHealthy()
    {

        GameObject newLobbyButton = GameObject.Find("NewLobbyButton");
        yield return CoClickButton(newLobbyButton);
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));
        yield return new WaitForSeconds(2f);

        GameObject launchGameButton = GameObject.Find("LaunchGameButton");
        yield return CoClickButton(launchGameButton);
        yield return new WaitForSeconds(2f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForFixedUpdate();

        Assert.That(SocketConnectionManager.Instance.isConnectionOpen, Is.EqualTo(true));
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
