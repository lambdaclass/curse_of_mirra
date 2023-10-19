using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;


public class MainScreenElements
{
    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScreen.unity");
    }

    [Test]
    public void MainCameraIsPresent()
    {
        var mainCamera = GameObject.Find("ModelCamera");
        Assert.That(mainCamera, Is.Not.Null);
    }

    [Test]
    public void CharactersButton()
    {
        var charactersButton = GameObject.Find("CharactersButton");
        Assert.That(charactersButton, Is.Not.Null);
    }

    [Test]
    public void LoadoutButton()
    {
        var loadoutButton = GameObject.Find("LoadoutButton");
        Assert.That(loadoutButton, Is.Not.Null);
    }

    [Test]
    public void PlayGameButton()
    {
        var playGameButton = GameObject.Find("PlayGameButton");
        Assert.That(playGameButton, Is.Not.Null);
    }

    [TearDown]
    public void Teardown()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}