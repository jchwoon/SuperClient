using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill
{
    public MyHero Owner { get; protected set; }
    public int TemplateId { get; protected set; }
    public SkillData SkillData { get; protected set; }
    public bool IsCoolTime {  get; protected set; }

    public BaseSkill(int templateId, MyHero owner, SkillData skillData)
    {
        TemplateId = templateId;
        Owner = owner;
        SkillData = skillData;
    }

    //서버에 보내기전 (SendUseSkill) 클라에서도 판단
    public ESkillFailReason CheckCanUseSkill()
    {
        if (IsCoolTime == true)
            return ESkillFailReason.Cool;

        return ESkillFailReason.None;
    }

    public virtual void UseSkill(string playAnimName)
    {
        Owner.Animator.Play(playAnimName);
        
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunCoolTime());
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunAnimTime());
    }

    IEnumerator CoRunCoolTime()
    {
        IsCoolTime = true;
        float coolTime = SkillData.CoolTime;
        float process = 0.0f;
        while (process < coolTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        IsCoolTime = false;
    }

    IEnumerator CoRunAnimTime()
    {
        Owner.SkillComponent.isUsingSkill = true;
        float animTime = SkillData.AnimTime;
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUsingSkill = false;
    }

    IEnumerator CoRunEffectTime()
    {
        float effectTime = SkillData.EffectDelayRatio * SkillData.AnimTime;
        float process = 0.0f;
        while (process < effectTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
    }
}

