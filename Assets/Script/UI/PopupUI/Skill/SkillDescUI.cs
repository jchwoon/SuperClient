using Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Enum;

public class SkillDescUI : BaseUI
{
    enum GameObjects
    {
        CurrentSkillDescPanel,
        NextSkillDescPanel
    }
    enum Images
    {
        SkillImage
    }

    enum Texts
    {
        SkillNameTxt,
        MasterLevelTxt,
        DescTxt,
        //CurrentLevel
        CurrentLevelLabelTxt,
        CostDescTxt,
        DetailDesctxt,
        CoolTimeDescTxt,
        //NextLevel
        NextLevelLabelTxt,
        NextCostDescTxt,
        NextLevelDetailDesctxt,
        NextCoolTimeDescTxt,
    }

    SkillData _skillData;
    int _skillLevel;

    GameObject _currentLevelDesc;
    GameObject _nextLevelDesc;

    Image _skillImage;

    TMP_Text _masterLevelTxt;
    TMP_Text _descTxt;
    TMP_Text _skillNameTxt;
    //Current Level
    TMP_Text _currentLevelLabelTxt;
    TMP_Text _costDescTxt;
    TMP_Text _detailDesctxt;
    TMP_Text _coolTimeDescTxt;
    //Next Level
    TMP_Text _nextLevelLabelTxt;
    TMP_Text _nextCostDescTxt;
    TMP_Text _nextLevelDetailDesctxt;
    TMP_Text _nextCoolTimeDescTxt;

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));

        _currentLevelDesc = Get<GameObject>((int)GameObjects.CurrentSkillDescPanel);
        _nextLevelDesc = Get<GameObject>((int)GameObjects.NextSkillDescPanel);

        _skillImage = Get<Image>((int)Images.SkillImage);

        _masterLevelTxt = Get<TMP_Text>((int)Texts.MasterLevelTxt);
        _descTxt = Get<TMP_Text>((int)Texts.DescTxt);
        _skillNameTxt = Get<TMP_Text>((int)Texts.SkillNameTxt);
        //CurrentLevel
        _currentLevelLabelTxt = Get<TMP_Text>((int)Texts.CurrentLevelLabelTxt);
        _costDescTxt = Get<TMP_Text>((int)Texts.CostDescTxt);
        _detailDesctxt = Get<TMP_Text>((int)Texts.DetailDesctxt);
        _coolTimeDescTxt = Get<TMP_Text>((int)Texts.CoolTimeDescTxt);
        //NextLevel
        _nextLevelLabelTxt = Get<TMP_Text>((int)Texts.NextLevelLabelTxt);
        _nextCostDescTxt = Get<TMP_Text>((int)Texts.NextCostDescTxt);
        _nextLevelDetailDesctxt = Get<TMP_Text>((int)Texts.NextLevelDetailDesctxt);
        _nextCoolTimeDescTxt = Get<TMP_Text>((int)Texts.NextCoolTimeDescTxt);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdateSkill, Refresh);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdateSkill, Refresh);
    }

    public void Refresh(SkillData skillData)
    {
        if (!IsValidSkillData(skillData)) return;
        Clear();
        //이미지 표시
        DisplaySkillImage(skillData);
        //스킬이름 표시
        DisplaySkillName(skillData);
        //스킬 마스터레벨 표시
        DisplayMasterLevel(skillData);
        //현재 스킬레벨 설명
        DisplayCurrentLevel(skillData);
        //다음 스킬레벨 설명
        DisplayNextLevelDetails(skillData);
    }

    private void Refresh()
    {
        if (_skillData == null)
            return;
        Refresh(_skillData);
    }

    private void DisplaySkillImage(SkillData skillData)
    {
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        _skillImage.gameObject.SetActive(true);
    }

    private void DisplaySkillName(SkillData skillData)
    {
        _skillNameTxt.text = skillData.SkillName;
    }

    private void DisplayMasterLevel(SkillData skillData)
    {
        _masterLevelTxt.text = $"마스터 레벨 : {skillData.MaxLevel}Lv";
    }

    private void DisplayCurrentLevel(SkillData skillData)
    {
        if (_skillLevel <= 0)
        {
            _currentLevelDesc.SetActive(false);
        }
        else
        {
            _currentLevelDesc.SetActive(true);
            _currentLevelLabelTxt.text = $"[현재레벨] {_skillLevel}";
            if (skillData.SkillType == ESkillType.Active)
            {
                ActiveSkillData activeSkillData = (ActiveSkillData)skillData;
                _costDescTxt.text = Fomatter.FormatSkillCost(activeSkillData);
                _coolTimeDescTxt.text = Fomatter.FormatSkillCool(activeSkillData);
            }
            _detailDesctxt.text = MakeSkillDetailDesc(skillData, _skillLevel);
        }
    }

    private void DisplayNextLevelDetails(SkillData skillData)
    {
        if (skillData.MaxLevel <= _skillLevel)
        {
            _nextLevelDesc.SetActive(false);
        }
        else
        {
            _nextLevelDesc.SetActive(true);
            _nextLevelLabelTxt.text = $"[다음레벨] {_skillLevel + 1}";

            if (skillData.SkillType == ESkillType.Active)
            {
                ActiveSkillData activeSkillData = (ActiveSkillData)skillData;
                _nextCostDescTxt.text = Fomatter.FormatSkillCost(activeSkillData);
                _nextCoolTimeDescTxt.text = Fomatter.FormatSkillCool(activeSkillData);
            }
            _nextLevelDetailDesctxt.text = MakeSkillDetailDesc(skillData, _skillLevel + 1);
        }
    }

    private string MakeSkillDetailDesc(SkillData skillData, int skillLevel)
    {
        string ret = "";
        foreach (int id in skillData.EffectIds)
        {
            Managers.DataManager.SkillEffectDict.TryGetValue(id, out EffectData effectData);
            Managers.DataManager.DescriptionDict.TryGetValue(effectData.DescId, out DescriptionData descData);
            if (!string.IsNullOrEmpty(descData.Text))
            {
                _descTxt.text = descData.Text;
            }
            if (effectData != null && descData != null)
            {
                ret += $"{Fomatter.FormatSkillDescription(skillData, descData, skillLevel, effectData)}";
            }
        }   
        return ret;
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

    private void Clear()
    {
        _costDescTxt.text = string.Empty;
        _coolTimeDescTxt.text = string.Empty;
        _nextLevelLabelTxt.text = string.Empty;
        _nextCostDescTxt.text = string.Empty;
        _nextLevelDetailDesctxt.text = string.Empty;
        _nextCoolTimeDescTxt.text = string.Empty;
    }
}
