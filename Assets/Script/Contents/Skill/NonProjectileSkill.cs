using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonProjectileSkill : BaseSkill
{
    public NonProjectileSkill(int skillId, MyHero owner, SkillData skillData) : base(skillId, owner, skillData)
    {
    }

    protected override IEnumerator CoAnimTime() 
    {
        Owner.SkillComponent.isUsingSkill = true;
        float animTime = SkillData.AnimTime * (1.0f / Owner.Stat.StatInfo.AtkSpeed);
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUsingSkill = false;
    }
}
