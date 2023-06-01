using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLoot : MonoBehaviour
{
    GameObject healthPrefab;


    // Start is called before the first frame update
    public void Init()
    {
        healthPrefab = Instantiate(Resources.Load("LootHealth", typeof(GameObject))) as GameObject;
        healthPrefab.transform.position = new Vector3(Random.Range(-40f, 50f), 1f, Random.Range(-40f, 50f));
        healthPrefab.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
