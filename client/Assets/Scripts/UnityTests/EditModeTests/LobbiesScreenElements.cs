using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class LobbiesScreenElements
{
    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Lobbies.unity");
    }

    [Test]
    public void MainCameraIsPresent()
    {
        var mainCamera = GameObject.Find("Main Camera");
        Assert.That(mainCamera, Is.Not.Null);
        Camera hasCameraComponent = mainCamera.GetComponent<Camera>();
        Assert.That(hasCameraComponent, Is.Not.Null);
    }

    [Test]
    public void NewLobbyButtonIsPresent()
    {
        var newLobbyButton = GameObject.Find("NewLobbyButton");
        Assert.That(newLobbyButton, Is.Not.Null);
    }

    [Test]
    public void RefreshButtonIsPresent()
    {
        var refreshButton = GameObject.Find("RefreshButton");
        Assert.That(refreshButton, Is.Not.Null);
    }

    [Test]
    public void QuickGameButtonIsPresent()
    {
        var quickGameButton = GameObject.Find("QuickGameButton");
        Assert.That(quickGameButton, Is.Not.Null);
    }

    [Test]
    public void ServerInfoContainerIsPresent()
    {
        var serverName = GameObject.Find("ServerName");
        Assert.That(serverName, Is.Not.Null);
    }

    [TearDown]
    public void Teardown()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
