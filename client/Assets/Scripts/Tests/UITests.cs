using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEditor.SceneManagement;

public class UITests
{
    [SetUp]
    public void Setup()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TitleScreen.unity");
    }

    [Test]
    public void VerifySceneElements()
    {
        var enterButton = GameObject.Find("Enter");
        Assert.That(enterButton, Is.Not.Null);
    }

    [TearDown]
    public void Teardown()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}
