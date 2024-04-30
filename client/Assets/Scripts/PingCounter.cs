using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

// SPIKES_LIST_MAX_LENGTH has the las value updated each updateInterval
// The way we determine connectivity issues is when the spikes counter is greater or equal than the spikesAmountThreshold
// TODO: adjust values so the feedback given is representative of a true connection issue 
// spikeValueThreshold: what is consider a standard spike?
// SPIKES_LIST_MAX_LENGTH: what should the list length be?
// updateInterval: how requent should the update be?
// spikesAmountThreshold: how many spikes should be in a list, of X length updated every Y seconds, to be considered unstable?

/// <summary>
/// Add this class to a gameObject with a Text component and it'll feed it the PING number in real time.
/// </summary>
public class PingCounter : MonoBehaviour
{
    [SerializeField]
    GameObject connectivityIcon;

    /// the frequency at which the PING counter should update
    public float updateInterval = 1f;
    protected float _timeLeftToUpdate;
    protected Text _pingText;

    public float spikeValueThreshold = 3f;
    public int spikesAmountThreshold = 4;
    
    // Queue<uint> pingValues = new Queue<uint>(10);
    List<uint> pingValues = new List<uint>();
    const int SPIKES_LIST_MAX_LENGTH = 10;

    /// <summary>
    /// On Start(), we get the Text component and initialize our counter
    /// </summary>
    protected virtual void Start()
    {
        if (GetComponent<Text>() == null)
        {
            Debug.LogWarning("PINGCounter requires a GUIText component.");
            return;
        }
        _pingText = GetComponent<Text>();
        _timeLeftToUpdate = updateInterval;
    }

    /// <summary>
    /// On Update, we decrease our time_left counter, and if we've reached zero, we update our PING counter
    /// with the last PING received
    /// </summary>
    protected virtual void Update()
    {
        uint currentPing = GameServerConnectionManager.Instance.currentPing;
        _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;
            
            _pingText.text = "PING " + currentPing.ToString();

            // Check if the list Length is already 10 and keep it that way
            if( pingValues.Count >= SPIKES_LIST_MAX_LENGTH){
                pingValues.RemoveAt(0);
            }
            pingValues.Add(currentPing);

            // Check if the target value exists in the list
            bool unstableConnection = SpikesStabilityCheck(pingValues);
            if(unstableConnection){
                print("More spikes than usual found in list: " + pingValues);
            }
        }
    }
    bool SpikesStabilityCheck(List<uint> list)
    {
        int spikesCounter = 0;
        for(int i = 0; i < list.Count; i++){
            if(i-1 >= 0){
                print("diff: " + Mathf.Abs(list[i] - list[i -1]));
                // Check for spikes
                if (Mathf.Abs(list[i] - list[i -1]) > spikeValueThreshold)
                {
                    print("Ping spike detected!");
                    spikesCounter += 1;
                }
            }
        }
        if (spikesCounter >=  spikesAmountThreshold) {
            return true;
        } else {
            return false;
        }
    }
}
