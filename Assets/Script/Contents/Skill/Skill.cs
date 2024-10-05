using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Skill
{
    public MyHero Owner { get; protected set; }
    public int SkillId { get; protected set; }
    public SkillData SkillData { get; protected set; }

    public Skill(int skillId, MyHero owner, SkillData skillData)
    {
        SkillId = skillId;
        Owner = owner;
        SkillData = skillData;
    }
}

public class NormalSkill : Skill
{
    public NormalSkill(int skillId, MyHero owner, SkillData skillData) : base(skillId, owner, skillData)
    {
    }
}
