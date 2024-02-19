using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorController : MonoBehaviour
{
    [SerializeField]
    private Transform root = null;

    [SerializeField]
    private GameObject middle_sector = null;

    [SerializeField]
    private Transform left_border_sector = null;

    [SerializeField]
    private Transform right_border_sector = null;

    private const int MIN_DEGREE_STEP = 5;
    private List<GameObject> spawned_sectors = new List<GameObject>();
    private int cached_degree = 0;
    private float offset = 2.5f;

    public void SetSectorDegree(float degree) => SetSectorDegree((int)degree);

    public void SetSectorDegree(int degree)
    {
        int sectors_count = degree / MIN_DEGREE_STEP;

        if (cached_degree == sectors_count * MIN_DEGREE_STEP)
            return;

        root.transform.localRotation = Quaternion.Euler(0.0f, (float)degree / 2, 0.0f);

        cached_degree = sectors_count * MIN_DEGREE_STEP;

        Clear();
        sectors_count -= 2;
        sectors_count = sectors_count < 0 ? 0 : sectors_count;

        left_border_sector.transform.localRotation = Quaternion.Euler(
            0.0f,
            (-(float)sectors_count * (float)MIN_DEGREE_STEP / 2) - offset,
            180.0f
        );
        right_border_sector.transform.localRotation = Quaternion.Euler(
            0.0f,
            ((float)sectors_count * (float)MIN_DEGREE_STEP / 2) + offset,
            0.0f
        );

        if (sectors_count <= 0)
            return;

        GameObject cached_sector = null;

        for (int i = 0; i < sectors_count; i++)
        {
            cached_sector = Instantiate(middle_sector, root);
            spawned_sectors.Add(cached_sector);

            float angle =
                ((-(float)sectors_count * (float)MIN_DEGREE_STEP / 2) + i * MIN_DEGREE_STEP)
                + offset;
            cached_sector.transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    private void Clear()
    {
        foreach (GameObject go in spawned_sectors)
            Destroy(go);

        spawned_sectors.Clear();
    }
}
