using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonProjectileSkill : ActiveSkill
{
    public NonProjectileSkill(int skillId, MyHero owner, ActiveSkillData skillData, int skillLevel) : base(skillId, owner, skillData, skillLevel)
    {
    }
}
