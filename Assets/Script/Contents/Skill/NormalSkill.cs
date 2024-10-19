using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSkill : BaseSkill
{
    Coroutine _comboRoutine;
    int _currentComboIdx = 0;
    Dictionary<int, int> _normalSkillComboHash = new Dictionary<int, int>();
    public NormalSkill(int skillId, MyHero owner, SkillData skillData) : base(skillId, owner, skillData)
    {
        for (int i = 0; i < skillData.ComboNames.Count; i++)
        {
            _normalSkillComboHash.Add(i, Animator.StringToHash(skillData.ComboNames[i]));
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();

        MyHeroStateMachine machine = Owner.MyHeroStateMachine;
        int currentComboHash = _normalSkillComboHash[_currentComboIdx];
        Owner.Animator.Play(currentComboHash);
        CoroutineHelper.Instance.StopHelperCoroutine(_comboRoutine);
        if (_currentComboIdx >= SkillData.MaxComboIdx)
            _currentComboIdx = 0;
        else
            _currentComboIdx++;
    }
    protected override IEnumerator CoAnimTime() 
    {
        Owner.SkillComponent.isUsingSkill = true;
        float animTime = SkillData.AnimTime * (1.0f / Owner.Stat.StatInfo.AtkSpeed);
        float process = 0.0f;
        while (process < animTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Owner.SkillComponent.isUsingSkill = false;
        StartComboTime();
    }

    private void StartComboTime()
    {
        _comboRoutine = CoroutineHelper.Instance.StartHelperCoroutine(CoComboTime());
    }

    IEnumerator CoComboTime()
    {
        float comboTime = SkillData.ComboTime;
        float process = 0.0f;
        while (process < comboTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        _currentComboIdx = 0;
    }
}
