using Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescUI : BaseUI
{
    enum GameObjects
    {
        NextSkillDescPanel
    }
    enum Images
    {
        SkillImage
    }

    enum Texts
    {
        RequiredLevelTxt,
        DescTxt,
        DetailDesctxt,
        NextLevelDetailDesctxt,
        SkillNameTxt,
        CurrentLevelLabelTxt,
        NextLevelLabelTxt
    }

    SkillData _skillData;
    int _skillLevel;

    GameObject _nextSkillDescPanel;

    Image _skillImage;

    TMP_Text _currentLevelLabelTxt;
    TMP_Text _nextLevelLabelTxt;
    TMP_Text _detailDesctxt;
    TMP_Text _requiredLevelTxt;
    TMP_Text _descTxt;
    TMP_Text _skillNameTxt;
    TMP_Text _nextLevelDetailDesctxt;

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));

        _nextSkillDescPanel = Get<GameObject>((int)GameObjects.NextSkillDescPanel);

        _skillImage = Get<Image>((int)Images.SkillImage);

        _currentLevelLabelTxt = Get<TMP_Text>((int)Texts.CurrentLevelLabelTxt);
        _nextLevelLabelTxt = Get<TMP_Text>((int)Texts.NextLevelLabelTxt);

        _detailDesctxt = Get<TMP_Text>((int)Texts.DetailDesctxt);
        _nextLevelDetailDesctxt = Get<TMP_Text>((int)Texts.NextLevelDetailDesctxt);

        _requiredLevelTxt = Get<TMP_Text>((int)Texts.RequiredLevelTxt);
        _descTxt = Get<TMP_Text>((int)Texts.DescTxt);
        _skillNameTxt = Get<TMP_Text>((int)Texts.SkillNameTxt);
    }

    public void Refresh(SkillData skillData)
    {
        if (!IsValidSkillData(skillData)) return;
        //이미지 표시
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        _skillImage.gameObject.SetActive(true);
        //스킬이름 표시
        _skillNameTxt.text = skillData.SkillName;
        //스킬 요구레벨 표시
        _requiredLevelTxt.text = $"마스터 레벨 : {skillData.MaxLevel}Lv";
        //스킬 설명 표시
        //현재 스킬레벨 설명
        Managers.DataManager.EffectDict.TryGetValue(skillData.EffectId, out EffectData effectData);
        Managers.DataManager.DescriptionDict.TryGetValue(skillData.DescId, out DescriptionData descData);
        _descTxt.text = descData.Text;
        if (effectData != null && descData != null)
        {
            _currentLevelLabelTxt.text = $"[현재레벨] {_skillLevel}";
            _detailDesctxt.text = Fomatter.FormatSkillDescription(skillData, descData, _skillLevel, effectData);
            if (skillData.MaxLevel > _skillLevel)
            {
                _nextSkillDescPanel.SetActive(true);
                _nextLevelLabelTxt.text = $"[다음레벨] {_skillLevel + 1}";
                _nextLevelDetailDesctxt.text = Fomatter.FormatSkillDescription(skillData, descData, _skillLevel + 1, effectData);
            }
            else
            {
                _nextSkillDescPanel.SetActive(false);
            }
        }
    }

    private bool IsValidSkillData(SkillData skillData)
    {
        if (skillData == null) return false;

        int skillLevel = Utils.GetMySkillComponent().GetSkillLevelById(skillData.TemplateId);
        if (_skillData != null && _skillData.TemplateId == skillData.TemplateId && _skillLevel == skillLevel)
        {
            return false;
        }
        _skillData = skillData;
        _skillLevel = skillLevel;

        return true;
    }
}
