using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillComponent
{
    Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
    public int NormalSkillId { get; private set; }
    public int DashSkillId { get; private set; }
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
                switch (skillData.SkillProjectileType)
                {
                    case ESkillProjectileType.None:
                        skill = new NonProjectileSkill(id, Managers.ObjectManager.MyHero, skillData);
                        break;
                }
                if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
                    NormalSkillId = skill.TemplateId;
                else if (skill.SkillData.SkillSlotType == ESkillSlotType.Dash)
                    DashSkillId = skill.TemplateId;
                if (skill != null)
                    _skills.Add(id, skill);
            }
        }
    }

    public BaseSkill GetSkillById(int templateId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(templateId, out skill) == false)
            return null;

        return skill;
    }

    public List<BaseSkill> GetAllSkill()
    {
        return _skills.Values.ToList();
    }
}
