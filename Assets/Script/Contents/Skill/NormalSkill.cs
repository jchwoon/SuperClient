using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSkill : BaseSkill
{
    public NormalSkill(int skillId, MyHero owner, SkillData skillData) : base(skillId, owner, skillData)
    {
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (Owner.MyHeroStateMachine.CurrentActiveSkillHash.HasValue && Owner.MyHeroStateMachine.CurrentActiveSkillHash.Value == AnimParamHash)
            Owner.MyHeroStateMachine.SetAnimParameter(Owner, Owner.AnimData.AttackComboHash, true);
    }
    protected override IEnumerator CoAnimTime()
    {
        Owner.SkillComponent.isUseSkill = true;
        float animTime = SkillData.AnimTime * Owner.StatInfo.AtkSpeed;
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUseSkill = false;
    }
}
