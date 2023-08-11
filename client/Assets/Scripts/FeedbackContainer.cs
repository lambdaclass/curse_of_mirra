using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackContainer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> feedbacksPrefabs;

    public void SetActiveFeedback(string name, bool active)
    {
        GameObject feedbackToActivate = feedbacksPrefabs.Find(el => el.name == name);
        feedbackToActivate.SetActive(active);
    }

    public List<GameObject> GetFeedbackList()
    {
        return feedbacksPrefabs;
    }
}
