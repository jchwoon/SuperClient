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
        //이미지 표시
        Image skillImage = Get<Image>((int)Images.SkillImage);
        skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        skillImage.gameObject.SetActive(true);
        //스킬이름 표시
        Get<TMP_Text>((int)Texts.SkillNameTxt).text = skillData.SkillName;
        //스킬 요구레벨 표시
        Get<TMP_Text>((int)Texts.RequiredLevelTxt).text = $"요구레벨 : {skillData.RequireLevel}Lv";
        //스킬 설명 표시
        EffectData effectData;
        DescriptionData descData;
        Managers.DataManager.EffectDict.TryGetValue(skillData.EffectId, out effectData);
        Managers.DataManager.DescriptionDict.TryGetValue(skillData.DescId, out descData);
        Get<TMP_Text>((int)Texts.DescTxt).text = descData.Text;
        //스킬 디테일 설명 표시
        if (effectData != null && descData != null)
        {
            Get<TMP_Text>((int)Texts.DetailDesctxt).text = Fomatter.FormatSkillDescription(skillData, descData, effectData);
        }
    }
}
