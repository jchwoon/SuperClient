using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public ActiveSkillData ActiveSkillData { get; protected set; }
    public bool IsCoolTime { get; protected set; }
    float _remainCoolTime;
    public ActiveSkill(int templateId, MyHero owner, ActiveSkillData skillData, int skillLevel) : base(templateId, owner, skillData, skillLevel)
    {
        ActiveSkillData = skillData;
    }

    public virtual void UseSkill()
    {
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunCoolTime());
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunAnimTime());
    }

    //서버에 보내기전 (SendUseSkill) 클라에서도 판단
    public ESkillFailReason CheckCanUseSkill()
    {
        if (IsCoolTime == true)
            return ESkillFailReason.Cool;

        return ESkillFailReason.None;
    }

    public float GetRemainCoolTime()
    {
        return _remainCoolTime;
    }

    IEnumerator CoRunCoolTime()
    {
        IsCoolTime = true;
        float coolTime = ActiveSkillData.CoolTime;
        float process = 0.0f;
        _remainCoolTime = coolTime;
        while (process < coolTime)
        {
            _remainCoolTime -= Time.deltaTime;
            process += Time.deltaTime;
            yield return null;
        }
        _remainCoolTime = 0.0f;
        IsCoolTime = false;
    }

    IEnumerator CoRunAnimTime()
    {
        Owner.SkillComponent.isUsingSkill = true;
        float animTime = ActiveSkillData.AnimTime;
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUsingSkill = false;
    }
}
