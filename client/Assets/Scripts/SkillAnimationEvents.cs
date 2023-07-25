using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimationEvents : MonoBehaviour
{
    private Skill skill;

    public void UpdateActiveSkill(Skill activeSkill)
    {
        print("Update active skill (activeskill)" + activeSkill);
        print("Update active skill (skill)" + activeSkill);
        skill = activeSkill;
    }

    public void EndSkillFeedback()
    {
        if (skill)
        {
            print("EndSkillFeedback (skill)" + skill);
            skill.EndSkillFeedback();
            skill = null;
        }
    }
}
