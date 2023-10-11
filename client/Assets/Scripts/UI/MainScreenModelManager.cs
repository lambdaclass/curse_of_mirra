using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainScreenModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [SerializeField]
    List<GameObject> playerModels;

    GameObject modelClone;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerModel = playerModels.Single(
            playerModel => playerModel.name.Contains("H4ck")
        );
        modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModel.transform.rotation,
            playerModelContainer.transform
        );
    }

    // Update is called once per frame
    void Update() { }
}
