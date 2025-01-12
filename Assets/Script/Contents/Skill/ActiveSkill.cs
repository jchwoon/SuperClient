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

    public virtual void UseSkill(string playAnimName)
    {
        Owner.Animator.Play(playAnimName);
        if (!string.IsNullOrEmpty(ActiveSkillData.SoundLabel))
        {
            Managers.SoundManager.PlaySFX(ActiveSkillData.SoundLabel, Owner.transform);
        }
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunCoolTime());
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunAnimTime());
        if (!string.IsNullOrEmpty(ActiveSkillData.PrefabName))
        {
            ParticleInfo info = new ParticleInfo
            (
                ActiveSkillData.PrefabName,
                Owner.transform,
                0
            );
            CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(Owner, info));
        }

        if (!string.IsNullOrEmpty(ActiveSkillData.HitPrefabName))
        {
            Vector2 skillCastDir = new Vector2(Owner.transform.forward.x, Owner.transform.forward.z).normalized;
            List<Creature> targets = GetSkillEffectedTargets(Owner.transform.position, skillCastDir);
            foreach (Creature target in targets)
            {
                ParticleInfo info = new ParticleInfo
                (
                    ActiveSkillData.HitPrefabName,
                    target.transform,
                    0
                );
                CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(target, info));
            }
        }
    }

    public List<Creature> GetSkillEffectedTargets(Vector3 skillPos, Vector2 skillCastDir)
    {
        List<Creature> effectedCreatures = new List<Creature>();
        int maxEntityCount = ActiveSkillData.MaxEntityCount;
        int currentCount = 0;

        switch (ActiveSkillData.SkillAreaType)
        {
            case ESkillAreaType.Area:
                List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
                foreach (Creature creature in creatures)
                {
                    if (creature.Machine == null || creature.Machine.CreatureState == ECreatureState.Die) continue;
                    //효과를 주는 최대 마릿 수 제한
                    if (maxEntityCount != 0 && currentCount >= maxEntityCount) break;
                    //피아식별 검사
                    if (CheckSkillUsageType(creature, ActiveSkillData.SkillUsageTargetType) == false) continue;
                    //거리 검사
                    float dist = Vector3.Distance(creature.transform.position, skillPos);
                    if (dist > ActiveSkillData.SkillRange) continue;
                    //Sector 검사
                    Vector3 dir = (creature.transform.position - skillPos).normalized;
                    float dotValue = Vector2.Dot(
                        new Vector2(dir.x, dir.z),
                        new Vector2(skillCastDir.x, skillCastDir.y));

                    float skillSectorValue = MathF.Cos(ActiveSkillData.SectorAngle * Mathf.Deg2Rad);
                    if (skillSectorValue <= dotValue)
                        effectedCreatures.Add(creature);
                }
                break;
            default:
                return effectedCreatures;
        }

        return effectedCreatures;
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

    private bool CheckSkillUsageType(Creature target, ESkillUsageTargetType usageType)
    {
        switch (usageType)
        {
            case ESkillUsageTargetType.Self:
                return target.ObjectId == Owner.ObjectId;
            //나중에 파티시스템 만들면 수정
            case ESkillUsageTargetType.Ally:
                return target.ObjectType == Owner.ObjectType;
            case ESkillUsageTargetType.Enemy:
                return target.ObjectType != Owner.ObjectType;
            default:
                return false;
        }
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

    IEnumerator CoRunEffectTime(Creature target, ParticleInfo info)
    {
        float effectDelayTime = ActiveSkillData.EffectDelayRatio * ActiveSkillData.AnimTime;
        float process = 0.0f;
        while (process < effectDelayTime)
        {
            process += Time.deltaTime;
            yield return null;
        }

        Managers.ObjectManager.SpawnParticle(info);
    }
}
