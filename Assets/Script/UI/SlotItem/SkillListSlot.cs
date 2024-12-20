using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillListSlot : BaseUI
{

    List<SkillSlot> _skills = new List<SkillSlot>();
    protected override void Awake()
    {
        SkillSlot[] slotUIComponents = GetComponentsInChildren<SkillSlot>();

        foreach (SkillSlot slotUI in slotUIComponents)
        {
            _skills.Add(slotUI);
        }
    }

    public void SetInfo(List<SkillData> skills)
    {
        List<SkillData> sortedSkills = skills.OrderBy(s => s.RequireLevel).ToList();

        int count = sortedSkills.Count;
        for (int i = 0; i <  count; i++)
        {
            _skills[i].SetInfo(sortedSkills[i]);
        }
    }
}
