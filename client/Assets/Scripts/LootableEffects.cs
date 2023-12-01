using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(LootableIdleAnimation());
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.time * 5) * 0.1f) + 1, transform.position.z);
        transform.Rotate(0, 0, 50 * Time.deltaTime);
        // transform.Rotate(Vector3.up, Time.deltaTime, Space.World);
    }
}
