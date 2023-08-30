using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackContainer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> feedbacksStatesPrefabs;

    [SerializeField]
    List<GameObject> feedbacksPrefabs;

    public void SetActiveStateFeedback(string name, bool active)
    {
        GameObject feedbackToActivate = feedbacksStatesPrefabs.Find(el => el.name == name);
        feedbackToActivate.SetActive(active);
    }

    public List<GameObject> GetFeedbackStateList()
    {
        return feedbacksStatesPrefabs;
    }

    public GameObject GetFeedback(string name)
    {
        GameObject feedback = feedbacksPrefabs.Find(el => el.name == name);
        return feedback;
    }

    public List<GameObject> GetFeedbackList()
    {
        return feedbacksPrefabs;
    }
}
