using System.Collections.Generic;
using UnityEngine;

// TODO: this could probably be a part of another class
public class CharacterFeedbackManager : MonoBehaviour
{
    [SerializeField]
    public UmaMarks umaMarks;

    public ulong currentMark = 0;

    List<ulong> marksApplied = new List<ulong>();

    public void DisplayUmaMarks(ulong markId)
    {
        umaMarks.gameObject.SetActive(true);
        if (!marksApplied.Contains(markId))
        {
            marksApplied.Add(markId);
        }
        UpdateMarkImage(marksApplied.Count);
        currentMark = markId;
    }

    public void RemoveMarks(ulong markId)
    {
        if (marksApplied.Count > 0)
        {
            if (marksApplied.Contains(markId))
            {
                marksApplied.Remove(markId);
                UpdateMarkImage(marksApplied.Count);
            }
        }
        if (marksApplied.Count == 0 && currentMark != 0)
        {
            umaMarks.gameObject.SetActive(false);
            marksApplied.Clear();
        }
    }

    private void UpdateMarkImage(int markCount)
    {
        umaMarks.SetImage(markCount);
    }
}
