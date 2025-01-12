using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;

public class SkillComponent
{
    Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
    Dictionary<int, ActiveSkill> _activeSkills = new Dictionary<int, ActiveSkill>();
    Dictionary<int, PassiveSkill> _passiveSkills = new Dictionary<int, PassiveSkill>();
    public int NormalSkillId { get; private set; }
    public int DashSkillId { get; private set; }
    public bool isUsingSkill { get; set; }
    public int SkillPoint { get; private set; }

    public void InitSkill(Dictionary<int, int> skills)
    {
        foreach (KeyValuePair<int/*templateId*/, int/*level*/> s in skills) 
        {
            if (Managers.DataManager.SkillDict.TryGetValue(s.Key, out SkillData skillData) == true)
            {
                if (_skills.ContainsKey(s.Key))
                    continue;

                BaseSkill skill = null;
                switch (skillData.SkillType)
                {
                    case ESkillType.Active:
                        ActiveSkillData activeSkillData = (ActiveSkillData)skillData;
                        if (activeSkillData.SkillProjectileType == ESkillProjectileType.None)
                        {
                            skill = new NonProjectileSkill(s.Key, Managers.ObjectManager.MyHero, activeSkillData, s.Value);
                        }
                        //ToDo Projectile
                        _activeSkills.Add(s.Key, (ActiveSkill)skill);
                        if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
                            NormalSkillId = skill.TemplateId;
                        if (skill.SkillData.SkillSlotType == ESkillSlotType.Dash)
                            DashSkillId = skill.TemplateId;
                        break;
                    case ESkillType.Passive:
                        PassiveSkillData passiveSkillData = (PassiveSkillData)skillData;
                        skill = new PassiveSkill(s.Key, Managers.ObjectManager.MyHero, passiveSkillData, s.Value);
                        _passiveSkills.Add(s.Key, (PassiveSkill)skill);
                        break;
                }
                if (skill != null)
                {
                    _skills.Add(s.Key, skill);
                }
            }
        }
    }

    public void InitSkillPoint(int point)
    {
        SetSkillPoint(point);
    }

    public bool CheckCanUseSkill(int templatedId)
    {
        ActiveSkill skill = GetActiveSkillById(templatedId);
        if (skill == null)
            return false;

        if (skill.ActiveSkillData.CanCancel == false && isUsingSkill)
            return false;

        if (skill.CheckCanUseSkill() != ESkillFailReason.None)
            return false;

        return true;
    }

    //Player LevelUp
    public void LevelUp(int prevLevel, int currentLevel)
    {
        int increasePoint = currentLevel - prevLevel;

        SetSkillPoint(increasePoint);

        Managers.EventBus.InvokeEvent(Enums.EventType.UpdateSkill);
    }

    #region Get / Set

    public List<BaseSkill> GetAllSkills()
    {
        return _skills.Values.ToList();
    }
    public BaseSkill GetSkillById(int templateId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(templateId, out skill) == false)
            return null;

        return skill;
    }

    public ActiveSkill GetActiveSkillById(int templateId)
    {
        ActiveSkill skill;
        if (_activeSkills.TryGetValue(templateId, out skill) == false)
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

    public void SetSkillPoint(int point)
    {
        SkillPoint = point;
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
        if (SkillPoint < usePoint)
            return;

        SendLevelUpPacket(templatedId);
    }
    private void SendLevelUpPacket(int templateId)
    {
        ReqLevelUpSkillToS levelUpPacket = new ReqLevelUpSkillToS();
        levelUpPacket.SkillId = templateId;

        Managers.NetworkManager.Send(levelUpPacket);
    }

    public void SendResetPointPacket()
    {
        ReqInitSkillPointToS initSkillPointPacket = new ReqInitSkillPointToS();
        Managers.NetworkManager.Send(initSkillPointPacket);
    }

    public void HandleUpdateSkillLevelAndPoint(List<SkillLevelInfo> levelInfos, int point, int cost)
    {
        foreach (SkillLevelInfo levelInfo in levelInfos)
        {
            BaseSkill skill = GetSkillById(levelInfo.SkillId);
            if (skill == null) continue;
            skill.UpdateSkillLevel(levelInfo.SkillLevel);
        }

        SetSkillPoint(point);

        MyHero hero = Managers.ObjectManager.MyHero;
        hero.CurrencyComponent.RemoveGold(cost);

        Managers.EventBus.InvokeEvent(Enums.EventType.UpdateSkill);
    }
    #endregion
}
