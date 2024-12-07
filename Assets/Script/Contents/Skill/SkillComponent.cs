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
                switch (skillData.SkillProjectileType)
                {
                    case ESkillProjectileType.None:
                        skill = new NonProjectileSkill(id, Managers.ObjectManager.MyHero, skillData);
                        break;
                }
                if (skill.SkillData.IsNormalSkill == true)
                    SetNormalSkillId(skill.TemplateId);
                if (skill != null)
                    _skills.Add(id, skill);
            }
        }
    }

    public int GetCurrentNormalSkillId()
    {
        //해당 노멀 스킬이 콤보가능한 스킬일 수 있기 때문에
        BaseSkill skill = GetSkillById(NormalSkillId);
       
        return skill.TemplateId;
    }

    public void SetNormalSkillId(int templateId)
    {
        NormalSkillId = templateId;
    }

    public BaseSkill GetSkillById(int templateId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(templateId, out skill) == false)
            return null;

        return skill;
    }
}
