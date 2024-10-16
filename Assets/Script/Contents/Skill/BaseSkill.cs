using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class BaseSkill
{
    public MyHero Owner { get; protected set; }
    public int SkillId { get; protected set; }
    public SkillData SkillData { get; protected set; }
    public bool isCoolTime {  get; protected set; }
    public int AnimParamHash { get; protected set; }

    public BaseSkill(int skillId, MyHero owner, SkillData skillData)
    {
        SkillId = skillId;
        Owner = owner;
        SkillData = skillData;
        AnimParamHash = Animator.StringToHash(skillData?.AnimParamName);
    }

    public ESkillFailReason CheckCanUseSkill(Creature target)
    {
        if (isCoolTime == true)
            return ESkillFailReason.Cool;
        float dist = Vector3.Distance(Owner.transform.position, target.transform.position);
        if (dist > GetSkillRange())
            return ESkillFailReason.Dist;

        return ESkillFailReason.None;
    }

    public float GetSkillRange()
    {
        return SkillData.SkillRange;
    }

    public virtual void UseSkill()
    {
        MyHeroStateMachine machine = Owner.MyHeroStateMachine;
        int? currentHash = machine.CurrentActiveSkillHash;
        if (currentHash.HasValue && currentHash.Value != AnimParamHash)
            machine.SetAnimParameter(Owner, currentHash.Value, false);
        machine.SetAnimParameter(Owner, AnimParamHash, true);
        machine.CurrentActiveSkillHash = AnimParamHash;
        CoroutineHelper.Instance.StartHelperCoroutine(CoAnimTime());
        CoroutineHelper.Instance.StartHelperCoroutine(CoCoolTime());
    }

    public virtual void ExitSkill()
    {

    }

    IEnumerator CoCoolTime()
    {
        isCoolTime = true;
        float coolTime = SkillData.CoolTime;
        float process = 0.0f;
        while (process < coolTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        isCoolTime = false;
    }
    protected virtual IEnumerator CoAnimTime()
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
}

