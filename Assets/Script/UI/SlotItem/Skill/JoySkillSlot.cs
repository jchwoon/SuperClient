using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class JoySkillSlot : BaseUI
{
    enum Images
    {
        SkillImage,
        CoolTimeImage
    }
    enum Texts
    {
        CoolTimeTxt
    }
    public SkillData SkillData { get; private set; }
    Image _skillImage;
    Image _skillCoolImage;
    TMP_Text _coolTimeTxt;
    BaseSkill _skill;
    protected override void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));

        _skillImage = Get<Image>((int)Images.SkillImage);
        _skillCoolImage = Get<Image>((int)Images.CoolTimeImage);
        _coolTimeTxt = Get<TMP_Text>((int)Texts.CoolTimeTxt);
    }

    private void Update()
    {
        if (_skill == null || SkillData == null)
        {
            ClearCoolInfo();
            return;
        }

        float remainCool = _skill.GetRemainCoolTime();
        Debug.Log(remainCool);
        if (remainCool  > 0)
        {
            _skillCoolImage.fillAmount = remainCool / SkillData.CoolTime;
            _coolTimeTxt.text = Mathf.CeilToInt(remainCool).ToString();
        }
        else
        {
            ClearCoolInfo();
        }
    }

    public void SetInfo(SkillData skillData)
    {
        if (skillData == null)
        {
            Clear();
        }
        else
        {
            SetSkill(skillData);
        }
        SkillData = skillData;
    }

    public void UseSkill()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero != null)
        {
            hero.UseSkill(SkillData.TemplateId);
        }
    }

    private void SetSkill(SkillData skillData)
    {
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        _skillImage.gameObject.SetActive(true);

        SkillComponent skillComponent = Utils.GetSkillComponent();
        if (skillComponent != null)
        {
            _skill = skillComponent.GetSkillById(skillData.TemplateId);
        }
    }

    private void Clear()
    {
        _skillImage.sprite = null;
        _skillImage.gameObject.SetActive(false);
        _skill = null;
    }

    private void ClearCoolInfo()
    {
        _skillCoolImage.fillAmount = 0;
        _coolTimeTxt.text = "";
    }
}
