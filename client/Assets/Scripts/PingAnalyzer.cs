using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class PingAnalyzer : MonoBehaviour
{
    // SPIKES_LIST_MAX_LENGTH has the las value updated each updateInterval
    // The way we determine connectivity issues is when the spikes counter is greater or equal than the spikesAmountThreshold
    // TODO: adjust values so the feedback given is representative of a true connection issue 
    // spikeValueThreshold: what is consider a standard spike?
    // SPIKES_LIST_MAX_LENGTH: what should the list length be?
    // updateInterval: how requent should the update be?
    // spikesAmountThreshold: how many spikes should be in a list, of X length updated every Y seconds, to be considered unstable?

    public bool unstableConnection;
    public string pingValue;
    /// the frequency at which the PING counter should update
    float updateInterval = .75f;
    protected float _timeLeftToUpdate;
    float spikeValueThreshold = 10f;
    int spikesAmountThreshold = 4;
    List<float> pingValues = new List<float>();
    const int SPIKES_LIST_MAX_LENGTH = 10;
    public static PingAnalyzer Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeLeftToUpdate = updateInterval;     
    }

    // Update is called once per frame
    void Update()
    {
        float currentPing = (float)GameServerConnectionManager.Instance.currentPing;
        _timeLeftToUpdate = _timeLeftToUpdate - Time.deltaTime;
        if (_timeLeftToUpdate <= 0.0)
        {
            _timeLeftToUpdate = updateInterval;
            
            pingValue = currentPing.ToString();

            // Check if the list Length is already 10 and keep it that way
            if( pingValues.Count >= SPIKES_LIST_MAX_LENGTH){
                pingValues.RemoveAt(0);
            }
            pingValues.Add(currentPing);

            // Check if the target value exists in the list
            unstableConnection = SpikesStabilityCheck(pingValues);
            if(unstableConnection){
                //print("More spikes than usual found in list: " + pingValues);
            }
        }
    }

    bool SpikesStabilityCheck(List<float> list)
    {
        int spikesCounter = 0;
        if(list.Count >= 2){
            for(int i = 0; i < list.Count - 1; i++){
                // Check for spikes
                //print("diff: " + Mathf.Abs(list[i] - list[i + 1]));
                if (Mathf.Abs(list[i] - list[i + 1]) >= spikeValueThreshold)
                {
                    spikesCounter += 1;
                    print("spike: " + Mathf.Abs(list[i] - list[i + 1]));
                }
            }
        }
        // print("spikesCounter: " + spikesCounter);
        return spikesCounter >=  spikesAmountThreshold;
    }
}
