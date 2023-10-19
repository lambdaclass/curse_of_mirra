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
    public void EnterButtonIsPresent()
    {
        var enterButton = GameObject.Find("Enter");
        Assert.That(enterButton, Is.Not.Null);
    }

    [Test]
    public void VersionInfoIsPresent()
    {
        var versionButton = GameObject.Find("Version");
        Assert.That(versionButton, Is.Not.Null);
    }

    [Test]
    public void CurseOfMyrraLogoIsPresent()
    {
        var logo = GameObject.Find("Logo");
        Assert.That(logo, Is.Not.Null);
    }

    [Test]
    public void SignOutButtonIsPresent()
    {
        var signOutButton = GameObject.Find("SignOutButton");
        Assert.That(signOutButton, Is.Not.Null);
    }

    [Test]
    public void UserAvatar()
    {
        var userAvatar = GameObject.Find("UserAvatar");
        Assert.That(userAvatar, Is.Not.Null);
    }

    [Test]
    public void UserName()
    {
        var userName = GameObject.Find("UserName");
        Assert.That(userName, Is.Not.Null);
    }

    [TearDown]
    public void Teardown()
    {
        EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    }
}