using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BattleTests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Lobbies");
    }

    // Creates a lobby in the Lobby scene
    [UnityTest]
    public IEnumerator PlayerMovement()
    {
        // Change to localhost server
        yield return TestingUtils.ForceClick("ServerNameContainer");
        yield return new WaitForSeconds(.1f);
        yield return TestingUtils.ForceClick("LocalHost");
        yield return new WaitForSeconds(.1f);

        yield return TestingUtils.ForceClick("NewLobbyButton");
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Lobby"));

        yield return TestingUtils.ForceClick("LaunchGameButton");
        yield return new WaitForSeconds(1f);

        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("CharacterSelection"));
        yield return new WaitForFixedUpdate();

        UICharacterItem H4ckUICharacterItem = GameObject.Find("H4ck").GetComponent<UICharacterItem>();
        Assert.That(H4ckUICharacterItem.selected, Is.EqualTo(false));
        yield return TestingUtils.ForceClick("H4ck");
        yield return new WaitForSeconds(1f);
        Assert.That(H4ckUICharacterItem.selected, Is.EqualTo(true));
        yield return new WaitForSeconds(1f);

        yield return TestingUtils.ForceClick("ConfirmButton");
        yield return new WaitForSeconds(1f);
        Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Battle"));
        yield return new WaitForSeconds(3f);

        #region Move Player
        
        // Get character initial position
        GameObject player = Utils.GetPlayer(1);
        Vector3 initialPlayerPosition = player.transform.position;

        GameObject movementJoystick = GameObject.Find("JoystickKnob");
        movementJoystick.GetComponent<LeftMMTouchJoystick>().RawValue = new Vector2(1, 1);
        yield return new WaitForSeconds(.5f);
        movementJoystick.GetComponent<LeftMMTouchJoystick>().RawValue = new Vector2(0, 0);

        Vector3 currentPlayerPosition = player.transform.position;

        Assert.Greater(currentPlayerPosition.x, initialPlayerPosition.x, "The player didn't move right.");
        Assert.Greater(currentPlayerPosition.z, initialPlayerPosition.z, "The player didn't move up.");

        #endregion
    }
}
