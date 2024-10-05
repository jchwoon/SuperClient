using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponent
{
    Dictionary<int, Skill> _skills = new Dictionary<int, Skill>();

    public void RegisterSkill(HeroData heroData)
    {
        foreach (int id in heroData.SkillIds)
        {
           if (Managers.DataManager.SkillDict.TryGetValue(id, out SkillData skillData) == true)
            {
                if (_skills.ContainsKey(id))
                    continue;

                Skill skill = null;
                skill = new NormalSkill(id, Managers.ObjectManager.MyHero, skillData);

                if (skill != null)
                    _skills.Add(id, skill);
            }
        }
    }

    public void UseSkill(int skillId)
    {

    }

    public void ReqUseSkill()
    {

    }
}
