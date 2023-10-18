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
        int index = Random.Range(0, playerModels.Count);
        string name = playerModels[index].name;
        GameObject playerModel = playerModels.Single(playerModel => playerModel.name == name);
        modelClone = Instantiate(
            playerModel,
            playerModelContainer.transform.position,
            playerModel.transform.rotation,
            playerModelContainer.transform
        );
    }
}
