using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Google.Protobuf.Enum;
using System;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class Creature : BaseObject
{
    CreatureHUD _creatureHUD;
    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }
    public StatComponent Stat { get; protected set; }
    public EffectComponent EffectComponent { get; private set; }
    public FloatingTextController FloatingTextController { get; protected set; }
    public int ShieldValue { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = transform.GetComponent<Animator>();
        AnimData = new AnimationData();
        FloatingTextController = Utils.GetOrAddComponent<FloatingTextController>(gameObject);
        EffectComponent = new EffectComponent(this);
        Stat = new StatComponent(this);
    }
    protected override void Start()
    {
        if (isMachineInit == false)
        {
            Machine = new CreatureMachine(this);
            isMachineInit = true;
        }
        base.Start();
    }

    protected virtual void OnDie()
    {
        Machine.OnDie();
        ClearTarget();
        RemoveHUD();
    }

    protected virtual void OnHit()
    {

    }

    protected override void OnRevival()
    {
        base.OnRevival();
    }

    protected void AddHUD()
    {
        if (_creatureHUD == null)
            _creatureHUD = Managers.UIManager.AddCreatureHUD(this);

        _creatureHUD.AddHUD(this);
    }
    protected void RemoveHUD()
    {
        if (_creatureHUD == null)
            return;

        _creatureHUD.RemoveHUD();
    }

    public void InvokeChangeHUD()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
    }

    #region Network Send
    public void SendUseSkill(int skillId, int skillTargetId)
    {
        Vector2 joystickDir = Managers.GameManager.MoveInput;
        float rotY = joystickDir == Vector2.zero
            ? transform.rotation.eulerAngles.y
            : Utils.GetAngleFromDir(joystickDir);

        ReqUseSkillToS skillPacket = new ReqUseSkillToS();
        SkillInfo skillInfo = new SkillInfo()
        {
            SkillId = skillId,
            SkillTargetId = skillTargetId,
        };
        //Temp
        PosInfo skillPivot = new PosInfo()
        {
            PosX = transform.position.x,
            PosY = transform.position.y,
            PosZ = transform.position.z,
            RotY = rotY,
        };
        skillPacket.SkillInfo = skillInfo;
        skillPacket.SkillPivot = skillPivot;
        Managers.NetworkManager.Send(skillPacket);
    }
    #endregion

    #region Network Receive

    #region Skill
    public void HandleUseSkill(Creature owner, ResUseSkillToC skillPacket)
    {
        ActiveSkillData skillData;
        if (Managers.DataManager.ActiveSkillDict.TryGetValue(skillPacket.SkillInfo.SkillId, out skillData) == false)
            return;

        GameObject go = Managers.ObjectManager.FindById(skillPacket.SkillInfo.SkillTargetId);
        if (go != null)
            owner.transform.LookAt(go.transform);

        owner.Animator.Play(skillData.AnimName);
        owner.Machine.UseSkill(skillData, skillPacket);

        //사운드
        if (!string.IsNullOrEmpty(skillData.SoundLabel))
        {
            Managers.SoundManager.PlaySFX(skillData.SoundLabel, owner.transform);
        }
        //스킬 이펙트
        if (!string.IsNullOrEmpty(skillData.PrefabName))
        {
            ParticleInfo info = new ParticleInfo
            (
                skillData.PrefabName,
                owner.transform,
                0
            );
            CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(skillData, info));
        }
        //스킬 Hit 이펙트
        if (!string.IsNullOrEmpty(skillData.HitPrefabName))
        {
            Vector2 skillCastDir = new Vector2(owner.transform.forward.x, owner.transform.forward.z).normalized;
            List<Creature> targets = SkillComponent.GetSkillEffectedTargets(owner, skillData, owner.transform.position, skillCastDir);
            foreach (Creature target in targets)
            {
                ParticleInfo info = new ParticleInfo
                (
                    skillData.HitPrefabName,
                    target.transform,
                    0
                );
                CoroutineHelper.Instance.StartHelperCoroutine(CoRunEffectTime(skillData, info));
            }
        }
    }

    IEnumerator CoRunEffectTime(ActiveSkillData skillData, ParticleInfo info)
    {
        float effectDelayTime = skillData.EffectDelayRatio * skillData.AnimTime;
        float process = 0.0f;
        while (process < effectDelayTime)
        {
            process += Time.deltaTime;
            yield return null;
        }

        Managers.ObjectManager.SpawnParticle(info);
    }
    #endregion

    public virtual void HandleModifyStat(StatInfo statInfo)
    {
        Stat.StatInfo.MergeFrom(statInfo);
        InvokeChangeHUD();
    }

    public virtual void HandleModifyOneStat(EStatType statType, float changedValue, float gapValue, EFontType fontType)
    {
        Stat.SetStat(statType, changedValue);

        switch (statType)
        {
            case EStatType.Hp:
                AddHUD();
                InvokeChangeHUD();
                FloatingTextController.RegisterOrSpawnText(gapValue, transform, fontType);
                break;
            default:
                break;
        }
    }

    public virtual void HandleDie(int killerId)
    {
        OnDie();
    }

    public void HandleApplyEffect(long effectId, int templateId)
    {
        if (Managers.DataManager.EffectDict.TryGetValue(templateId, out EffectData effectData) == false)
            return;

        EffectComponent.ApplyEffect(effectData, effectId);
    }

    public void HandleReleaseEffect(long effectId)
    {
        EffectComponent.ReleaseEffect(effectId);
    }

    public void HandleChangedShield(int shieldValue)
    {
        ShieldValue = shieldValue;
        InvokeChangeHUD();
    }
    #endregion
}
