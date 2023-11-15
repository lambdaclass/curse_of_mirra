using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public IEnumerator IsServerHealthy()
    {
        yield return SetupLocalhostAsServer();
        yield return TestingUtils.ForceClick("NewLobbyButton");
        yield return new WaitForSeconds(2f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));

        yield return TestingUtils.ForceClick("LaunchGameButton");
        yield return new WaitForSeconds(2f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForSeconds(2f);

        Assert.That(SocketConnectionManager.Instance.isConnectionOpen, Is.EqualTo(true));
    }

    IEnumerator SetupLocalhostAsServer() {
        yield return TestingUtils.ForceClick("ServerNameContainer");
        yield return new WaitForSeconds(.1f);
        yield return TestingUtils.ForceClick("LocalHost");
        yield return new WaitForSeconds(.1f);
    }
}
