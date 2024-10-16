using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponent
{
    Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
    Dictionary<int, BaseSkill> _reservedSkills = new Dictionary<int, BaseSkill>();
    public int NormalSkillId { get; private set; }
    public bool isUsingSkill { get; set; }

    public void InitSkill(HeroData heroData)
    {
        foreach (int id in heroData.SkillIds)
        {
           if (Managers.DataManager.SkillDict.TryGetValue(id, out SkillData skillData) == true)
            {
                if (_skills.ContainsKey(id))
                    continue;

                BaseSkill skill = null;
                switch (skillData.SkillType)
                {
                    case ESkillType.Normal:
                        skill = new NormalSkill(id, Managers.ObjectManager.MyHero, skillData);
                        NormalSkillId = skill.SkillId;
                        break;
                }
                if (skill != null)
                    _skills.Add(id, skill);
            }
        }
    }

    //예약된 스킬들 중에서 사용이 가능한 스킬을 뽑아 옴
    public BaseSkill GetCanUseSkillAtReservedSkills(Creature target)
    {
        if (_reservedSkills.Count == 0)
        {
            BaseSkill normalSkill = GetSkillById(NormalSkillId);
            ESkillFailReason reason = normalSkill.CheckCanUseSkill(target);
            if (reason == ESkillFailReason.None)
                return normalSkill;
        }
        
        foreach(BaseSkill skill in _reservedSkills.Values)
        {
            ESkillFailReason reason =  skill.CheckCanUseSkill(target);
            if (reason == ESkillFailReason.None)
                return skill;
        }

        return null;
    }

    public ESkillFailReason CheckCanUseSkill(Creature target, BaseSkill skill = null)
    {
        BaseSkill useSkill = skill;
        if (useSkill == null)
            useSkill = _skills[NormalSkillId];

        return useSkill.CheckCanUseSkill(target);
    }

    public float GetSkillRange(BaseSkill skill)
    {
        return skill.GetSkillRange();
    }

    public BaseSkill GetSkillById(int skillId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(skillId, out skill) == false)
            return null;

        return skill;
    }
}
