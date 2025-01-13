using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : BaseSkill
{
    public PassiveSkillData PassiveSkillData { get; protected set; }
    public PassiveSkill(int templateId, MyHero owner, PassiveSkillData skillData, int skillLevel) : base(templateId, owner, skillData, skillLevel)
    {
        PassiveSkillData = skillData;
    }


}
