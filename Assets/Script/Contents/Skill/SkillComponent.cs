using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class SkillComponent
{
    Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
    public int NormalSkillId { get; private set; }
    public int DashSkillId { get; private set; }
    public bool isUsingSkill { get; set; }
    public int ActiveSkillPoint { get; private set; }
    public int PassiveSkillPoint { get; private set; }

    public void InitSkill(Dictionary<int, int> skills)
    {
        foreach (KeyValuePair<int/*templateId*/, int/*level*/> s in skills) 
        {
            if (Managers.DataManager.SkillDict.TryGetValue(s.Key, out SkillData skillData) == true)
            {
                if (_skills.ContainsKey(s.Key))
                    continue;

                BaseSkill skill = null;
                switch (skillData.SkillProjectileType)
                {
                    case ESkillProjectileType.None:
                        skill = new NonProjectileSkill(s.Key, Managers.ObjectManager.MyHero, skillData, s.Value);
                        break;
                }
                if (skill != null)
                    _skills.Add(s.Key, skill);
                if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
                    NormalSkillId = skill.TemplateId;
                else if (skill.SkillData.SkillSlotType == ESkillSlotType.Dash)
                    DashSkillId = skill.TemplateId;
            }
        }
    }

    public void InitSkillPoint(int activePoint, int passivePoint)
    {
        SetSkillPoint(ESkillType.Active, activePoint);
        SetSkillPoint(ESkillType.Passive, passivePoint);
    }

    public bool CheckCanUseSkill(int templatedId)
    {
        BaseSkill skill = GetSkillById(templatedId);
        if (skill == null)
            return false;

        if (skill.SkillData.CanCancel == false && isUsingSkill)
            return false;

        if (skill.CheckCanUseSkill() != ESkillFailReason.None)
            return false;

        return true;
    }

    //Player LevelUp
    public void LevelUp(int prevLevel, int currentLevel)
    {
        int increasePoint = currentLevel - prevLevel;
        ESkillType active = ESkillType.Active;
        ESkillType passive = ESkillType.Passive;

        SetSkillPoint(active, GetSkillPointBySkillType(active) + increasePoint);
        SetSkillPoint(passive, GetSkillPointBySkillType(passive) + increasePoint);

        Managers.EventBus.InvokeEvent(Enums.EventType.UpdateSkill);
    }

    #region Get / Set

    public BaseSkill GetSkillById(int templateId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(templateId, out skill) == false)
            return null;

        return skill;
    }

    public SkillData GetSkillDataById(int templateId)
    {
        BaseSkill skill = GetSkillById(templateId);
        if (skill == null)
            return null;

        return skill.SkillData;
    }

    public int GetSkillLevelById(int templatedId)
    {
        return GetSkillById(templatedId).CurrentSkillLevel;
    }

    public int GetSkillPointBySkillType(ESkillType skillType)
    {
        return skillType == ESkillType.Active ? ActiveSkillPoint : PassiveSkillPoint;
    }

    public void SetSkillPoint(ESkillType skillType, int point)
    {
        if (skillType == ESkillType.Active)
            ActiveSkillPoint = point;
        if (skillType == ESkillType.Passive)
            PassiveSkillPoint = point;
    }
    #endregion

    #region Network
    public void CheckAndSendLevelUpPacket(int templatedId, int usePoint = 1)
    {
        BaseSkill skill = GetSkillById(templatedId);
        if (skill == null)
            return;
        if (skill.CheckCanLevelUp(usePoint) == false)
            return;
        //스킬 포인트가 유효한지
        if (GetSkillPointBySkillType(skill.SkillData.SkillType) < usePoint)
            return;

        SendLevelUpPacket(templatedId);
    }
    private void SendLevelUpPacket(int templateId)
    {
        ReqLevelUpSkillToS levelUpPacket = new ReqLevelUpSkillToS();
        levelUpPacket.SkillId = templateId;

        Managers.NetworkManager.Send(levelUpPacket);
    }

    public void CheckAndSendResetPointPacket(ESkillType skillType)
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        if (Managers.DataManager.CostDict.TryGetValue(hero.HeroData.SkillInitId, out CostData costData) == false)
            return;

        if (hero.CurrencyComponent.CheckEnoughCurrency(costData.CurrencyType, costData.CostValue) == false)
            return;
        
        SendResetPointPacket(skillType);
    }
    private void SendResetPointPacket(ESkillType skillType)
    {
        ReqInitSkillPointToS initSkillPointPacket = new ReqInitSkillPointToS();
        initSkillPointPacket.SkillType = skillType;
        Managers.NetworkManager.Send(initSkillPointPacket);
    }

    public void HandleUpdateSkillLevelAndPoint(List<SkillLevelInfo> levelInfos, int activePoint, int passivePoint)
    {
        foreach (SkillLevelInfo levelInfo in levelInfos)
        {
            BaseSkill skill = GetSkillById(levelInfo.SkillId);
            if (skill == null) continue;
            skill.UpdateSkillLevel(levelInfo.SkillLevel);
        }

        SetSkillPoint(ESkillType.Active, activePoint);
        SetSkillPoint(ESkillType.Passive, passivePoint);

        Managers.EventBus.InvokeEvent(Enums.EventType.UpdateSkill);
    }
    #endregion
}
