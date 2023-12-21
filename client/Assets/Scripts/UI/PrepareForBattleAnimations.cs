using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class PrepareForBattleAnimations : MonoBehaviour
{
    [SerializeField]
    GameObject battleScreen,
        prepareBattleContainer,
        surviveContainer,
        playerCard,
        playersTable,
        ray;

    CinemachineFramingTransposer cameraFramingTransposer = null;

    void Start()
    {
        StartCoroutine(CameraCinematic());
    }

    IEnumerator CameraCinematic()
    {
        if (!SocketConnectionManager.Instance.cinematicDone)
        {
            float effectTime = Utils
                .GetCharacter(1)
                .characterBase.spawnFeedback.GetComponent<VisualEffect>()
                .GetFloat("LifeTime");
            //Start moving camera and remove loading sceen
            InvokeRepeating("Substract", 1f, 0.1f);
            yield return new WaitForSeconds(1.7f);
            this.gameObject.SetActive(false);
            battleScreen.SetActive(true);
            // Cancel camera movement and start zoom in
            yield return new WaitForSeconds(2.1f);
            CancelInvoke("Substract");
            InvokeRepeating("MoveYCamera", 0.3f, 0.1f);
            Utils
                .GetAllCharacters()
                .ForEach(character =>
                {
                    character.characterBase.ToggleSpawnFeedback(true, character.PlayerID);
                });
            yield return new WaitForSeconds(effectTime);
            Utils
                .GetAllCharacters()
                .ForEach(character =>
                {
                    character.characterBase.ToggleSpawnFeedback(false, character.PlayerID);
                });
            //Cancel camera zoom
            yield return new WaitForSeconds(0.5f);
            CancelInvoke("MoveYCamera");
        }
        else
        {
            cameraFramingTransposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.9f);
            this.gameObject.SetActive(false);
        }
    }
}
