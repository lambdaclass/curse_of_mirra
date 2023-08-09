using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimationEvents : MonoBehaviour
{
    private Skill skill;
    string animationId;

    public void UpdateActiveSkill(Skill activeSkill, string skillAnimationId)
    {
        skill = activeSkill;
        animationId = skillAnimationId;
    }

    public void EndSkillFeedback()
    {
        if (skill)
        {
            skill.EndSkillFeedback(animationId);
            skill = null;
            animationId = null;
        }
    }

    public IEnumerator TryEjectAnimation(Skill skill, string skillAnimationId, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (this.skill == skill && this.animationId == skillAnimationId)
        {
            skill.EndSkillFeedback(skillAnimationId);
        }
    }
}
