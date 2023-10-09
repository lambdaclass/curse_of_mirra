using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenModelManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerModelContainer;

    [SerializeField]
    GameObject playerModel;

    GameObject modelClone;

    // Start is called before the first frame update
    void Start()
    {
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
