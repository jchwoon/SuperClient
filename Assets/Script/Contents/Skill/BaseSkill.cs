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
    public int SkillLevel { get; protected set; }

    float _remainCoolTime;

    public BaseSkill(int templateId, MyHero owner, SkillData skillData, int skillLevel)
    {
        TemplateId = templateId;
        Owner = owner;
        SkillData = skillData;
        SkillLevel = skillLevel;
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
        if (!string.IsNullOrEmpty(SkillData.PrefabName))
        {
            CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(Owner.ObjectId));
        }
    }

    public float GetRemainCoolTime()
    {
        return _remainCoolTime;
    }

    IEnumerator CoRunCoolTime()
    {
        IsCoolTime = true;
        float coolTime = SkillData.CoolTime;
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
        float animTime = SkillData.AnimTime;
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUsingSkill = false;
    }

    IEnumerator CoRunEffectTime(int effectTargetId)
    {
        float effectDelayTime = SkillData.EffectDelayRatio * SkillData.AnimTime;
        float process = 0.0f;
        while (process < effectDelayTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        GameObject target = Managers.ObjectManager.FindById(effectTargetId);
        float effectDuration = SkillData.Duration == 0 ? SkillData.AnimTime - effectDelayTime : SkillData.Duration;
        ParticleInfo info = new ParticleInfo
            (
                SkillData.PrefabName,
                target != null ? target.transform : Owner.transform,
                effectDuration
            );
        Managers.ObjectManager.SpawnParticle(info);
    }
}

