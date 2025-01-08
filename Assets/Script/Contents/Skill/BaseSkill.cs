using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseSkill
{
    public MyHero Owner { get; protected set; }
    public int TemplateId { get; protected set; }
    public SkillData SkillData { get; protected set; }
    public bool IsCoolTime {  get; protected set; }
    public int CurrentSkillLevel { get; protected set; }

    float _remainCoolTime;

    public BaseSkill(int templateId, MyHero owner, SkillData skillData, int skillLevel)
    {
        TemplateId = templateId;
        Owner = owner;
        SkillData = skillData;
        CurrentSkillLevel = skillLevel;
    }

    //서버에 보내기전 (SendUseSkill) 클라에서도 판단
    public ESkillFailReason CheckCanUseSkill()
    {
        if (IsCoolTime == true)
            return ESkillFailReason.Cool;

        return ESkillFailReason.None;
    }

    public bool CheckCanLevelUp(int point)
    {
        int maxLevel = SkillData.MaxLevel;

        if (maxLevel < CurrentSkillLevel + point)
            return false;

        return true;
    }

    public virtual void UseSkill(string playAnimName)
    {
        Owner.Animator.Play(playAnimName);
        if (!string.IsNullOrEmpty(SkillData.SoundLabel))
        {
            Managers.SoundManager.PlaySFX(SkillData.SoundLabel, Owner.transform);
        }
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunCoolTime());
        CoroutineHelper.Instance.StartHelperCoroutine(CoRunAnimTime());
        if (!string.IsNullOrEmpty(SkillData.PrefabName))
        {
            ParticleInfo info = new ParticleInfo
            (
                SkillData.PrefabName,
                Owner.transform,
                0
            );
            CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(Owner, info));
        }

        if (!string.IsNullOrEmpty(SkillData.HitPrefabName))
        {
            Vector2 skillCastDir = new Vector2(Owner.transform.forward.x, Owner.transform.forward.z).normalized;
            List<Creature> targets = GetSkillEffectedTargets(Owner.transform.position, skillCastDir);
            foreach (Creature target in targets)
            {
                ParticleInfo info = new ParticleInfo
                (
                    SkillData.HitPrefabName,
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
        int maxEntityCount = SkillData.MaxEntityCount;
        int currentCount = 0;

        switch (SkillData.SkillAreaType)
        {
            case ESkillAreaType.Area:
                List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
                foreach (Creature creature in creatures)
                {
                    if (creature.Machine == null || creature.Machine.CreatureState == ECreatureState.Die) continue;
                    //효과를 주는 최대 마릿 수 제한
                    if (maxEntityCount != 0 && currentCount >= maxEntityCount) break;
                    //피아식별 검사
                    if (CheckSkillUsageType(creature, SkillData.SkillUsageTargetType) == false) continue;
                    //거리 검사
                    float dist = Vector3.Distance(creature.transform.position, skillPos);
                    if (dist > SkillData.SkillRange) continue;
                    //Sector 검사
                    Vector3 dir = (creature.transform.position - skillPos).normalized;
                    float dotValue = Vector2.Dot(
                        new Vector2(dir.x, dir.z),
                        new Vector2(skillCastDir.x, skillCastDir.y));

                    float skillSectorValue = MathF.Cos(SkillData.SectorAngle * Mathf.Deg2Rad);
                    if (skillSectorValue <= dotValue)
                        effectedCreatures.Add(creature);
                }
                break;
            default:
                return effectedCreatures;
        }

        return effectedCreatures;
    }

    public float GetRemainCoolTime()
    {
        return _remainCoolTime;
    }

    public void UpdateSkillLevel(int level)
    {
        CurrentSkillLevel = level;
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

    IEnumerator CoRunEffectTime(Creature target, ParticleInfo info)
    {
        float effectDelayTime = SkillData.EffectDelayRatio * SkillData.AnimTime;
        float process = 0.0f;
        while (process < effectDelayTime)
        {
            process += Time.deltaTime;
            yield return null;
        }

        Managers.ObjectManager.SpawnParticle(info);
    }
}

