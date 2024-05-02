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
    float updateInterval = 1f;
    protected float _timeLeftToUpdate;
    protected Text _pingText;

    public float spikeValueThreshold = 150f;
    public int spikesAmountThreshold = 4;
    
    // Queue<float> pingValues = new Queue<float>(10);
    List<float> pingValues = new List<float>();
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
        float currentPing = (float)GameServerConnectionManager.Instance.currentPing;
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
    bool SpikesStabilityCheck(List<float> list)
    {
        int spikesCounter = 0;
        if(list.Count >= 2){
            for(int i = 0; i < list.Count - 1; i++){
                // Check for spikes
                if (Mathf.Abs(list[i] - list[i + 1]) >= spikeValueThreshold)
                {
                    print("diff: " + Mathf.Abs(list[i] - list[i + 1]));
                    spikesCounter += 1;
                }
            }
        }
        print("spikesCounter: " + spikesCounter);
        if (spikesCounter >=  spikesAmountThreshold) {
            spikesCounter = 0;
            return true;
        } else {
            spikesCounter = 0;
            return false;
        }
    }
}
