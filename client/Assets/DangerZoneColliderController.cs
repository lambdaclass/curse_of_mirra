using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneColliderController : MonoBehaviour
{
    void Start()
    {
        // StartCoroutine(SetupCollitions());
    }

    IEnumerator SetupCollitions()
    {
        yield return new WaitUntil(() => SocketConnectionManager.Instance.players.Count != 0);
        Utils
            .GetAllCharacters()
            .ForEach(character =>
            {
                Physics.IgnoreCollision(
                    this.GetComponent<MeshCollider>(),
                    character.GetComponent<CharacterController>()
                );
            });
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ZoneParticle")
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
