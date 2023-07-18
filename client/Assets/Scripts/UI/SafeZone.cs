using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public static SafeZone instance;
    public float safeZoneRadius;
    public SpriteMask safeZone;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        safeZone = GetComponentInChildren<SpriteMask>();
        safeZoneRadius = 200f;
    }

    private void Update()
    {
        safeZoneRadius -= Time.deltaTime * 5f;
        safeZone.transform.localScale = new Vector3(safeZoneRadius, safeZoneRadius, 1);
    }
}
