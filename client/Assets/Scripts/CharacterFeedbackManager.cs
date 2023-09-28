using System.Collections.Generic;
using UnityEngine;

// TODO: this could probably be a part of another class
public class CharacterFeedbackManager : MonoBehaviour
{
    [SerializeField]
    public UmaMarks umaMarks;

    IDictionary<ulong, ISet<PlayerEffect>> playersMarks =
        new Dictionary<ulong, ISet<PlayerEffect>>();

    public void DisplayEffectMark(ulong playerId, PlayerEffect effect)
    {
        umaMarks.gameObject.SetActive(true);
        if (playersMarks.ContainsKey(playerId))
        {
            playersMarks[playerId].Add(effect);
        }
        else
        {
            playersMarks.Add(playerId, new HashSet<PlayerEffect>() { effect });
        }
        UpdateMarkImage(playersMarks[playerId].Count);
    }

    public void RemoveMark(ulong playerId, PlayerEffect effect)
    {
        if (playersMarks.ContainsKey(playerId))
        {
            playersMarks[playerId].Remove(effect);
            UpdateMarkImage(playersMarks[playerId].Count);
        }
    }

    private void UpdateMarkImage(int markCount)
    {
        umaMarks.SetImage(markCount);
    }
}
