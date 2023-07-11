using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimationEvents : MonoBehaviour
{
    private Skill skill;

    public void UpdateActiveSkill(Skill activeSkill)
    {
        skill = activeSkill;
    }

    public void EndSkillFeedback()
    {
        if (skill)
        {
            skill.EndSkillFeedback();
            skill = null;
        }
    }
}
