using Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescUI : BaseUI
{
    enum Images
    {
        SkillImage
    }

    enum Texts
    {
        RequiredLevelTxt,
        DescTxt,
        DetailDesctxt,
        SkillNameTxt
    }

    protected override void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));
    }

    public void OnSlotClicked(SkillData skillData)
    {
        //�̹��� ǥ��
        Image skillImage = Get<Image>((int)Images.SkillImage);
        skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        skillImage.gameObject.SetActive(true);
        //��ų�̸� ǥ��
        Get<TMP_Text>((int)Texts.SkillNameTxt).text = skillData.SkillName;
        //��ų �䱸���� ǥ��
        Get<TMP_Text>((int)Texts.RequiredLevelTxt).text = $"�䱸���� : {skillData.RequireLevel}Lv";
        //��ų ���� ǥ��
        EffectData effectData;
        DescriptionData descData;
        Managers.DataManager.EffectDict.TryGetValue(skillData.EffectId, out effectData);
        Managers.DataManager.DescriptionDict.TryGetValue(skillData.DescId, out descData);
        Get<TMP_Text>((int)Texts.DescTxt).text = descData.Text;
        //��ų ������ ���� ǥ��
        if (effectData != null && descData != null)
        {
            Get<TMP_Text>((int)Texts.DetailDesctxt).text = Fomatter.FormatSkillDescription(skillData, descData, effectData);
        }
    }
}
