using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MoreMountains.TopDownEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SpawnModel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject model;

    [SerializeField]
    CinemachineVirtualCamera camera;

    [SerializeField]
    Light mainLight;

    [SerializeField]
    Light rimLight;

    int count = 0;

    public void Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject ins = Instantiate(
                model,
                new Vector3(
                    UnityEngine.Random.Range(-5.0f, 5.0f),
                    0,
                    UnityEngine.Random.Range(-5.0f, 5.0f)
                ),
                Quaternion.identity
            );

            ins.transform.localScale *= 1.5f;
            camera.Follow = ins.transform;

            count++;
            this.GetComponentInChildren<TextMeshProUGUI>().text = model.name + " " + count;
        }
    }

    public void ToggleShadow()
    {
        this.GetComponentInChildren<TextMeshProUGUI>().text = mainLight.shadows.ToString();
        if (mainLight.shadows == LightShadows.None)
        {
            mainLight.shadows = LightShadows.Soft;
            rimLight.shadows = LightShadows.Soft;
        }
        else
        {
            mainLight.shadows = LightShadows.None;
            rimLight.shadows = LightShadows.None;
        }
    }
}
