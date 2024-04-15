using System.Collections;
using UnityEngine;

public class VFXGraphPrewarmer : MonoBehaviour
{
    [SerializeField] private VFXGraphHolder vfx_graph_holder = null;
    [SerializeField] private Transform spawn_root = null;
    [SerializeField] private Camera camera = null;
    [SerializeField] private int start_frames_delay = 5;
    [SerializeField] private int interval_frames_delay = 2;

    void Start()
    {
        StartCoroutine(spawnQueue());
    }

    IEnumerator spawnQueue()
    {
        float start_time = Time.time;
        GameObject cached_game_object = null;
        camera.enabled = true;

        yield return waitForFrames(start_frames_delay);
        foreach(GameObject vfx_graph in vfx_graph_holder.vfx_graphs)
        {
            if (vfx_graph == null)
                continue;

            cached_game_object = Instantiate(vfx_graph, spawn_root);
            yield return waitForFrames(interval_frames_delay);
            Destroy( cached_game_object );
        }
        camera.enabled = false;

        Debug.Log($"VFX Graph prewarming took {Time.time - start_time}s");
    }

    IEnumerator waitForFrames( int frame_count )
    {
        for(int i = 0; i < frame_count; i++ )
            yield return null;
    }
}
