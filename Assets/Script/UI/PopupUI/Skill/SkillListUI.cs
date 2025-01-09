using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillListUI : BaseUI
{
    enum GameObjects
    {
        SkillType,
        ActiveSkillContent,
        ActiveTab,
        PassiveTab,
        ActivePointInitialBtn,
        PassivePointInitialBtn
    }

    enum Texts
    {
        ActiveSkillPointTxt,
        PassiveSkillPointTxt
    }

    enum Toggles
    {
        ActiveToggle,
        PassiveToggle
    }

    GameObject _activeSkillContent;
    GameObject _activeSkillTab;
    GameObject _passiveSkillTab;

    Toggle _activeToggle;
    Toggle _passiveToggle;

    List<SkillData> _activeSkills;
    List<SkillData> _passiveSkills;

    Action<SkillData> _slotClickEvent;
    Action _skillRegisterEvent;
    Action<Sprite> _slotBeginDragEvent;
    Action<Vector2> _slotDragEvent;

    Dictionary<ESkillSlotType, SkillData> _skillsSortBySlotType = new Dictionary<ESkillSlotType, SkillData>(3)
    {
        {ESkillSlotType.Active1, null },
        {ESkillSlotType.Active2, null },
        {ESkillSlotType.Active3, null },
        {ESkillSlotType.Active4, null },
    };

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));
        Bind<TMP_Text>(typeof(Texts));

        _activeToggle = Get<Toggle>((int)Toggles.ActiveToggle);
        _passiveToggle = Get<Toggle>((int)Toggles.PassiveToggle);

        _activeSkillContent = Get<GameObject>((int)GameObjects.ActiveSkillContent);
        _activeSkillTab = Get<GameObject>((int)GameObjects.ActiveTab);
        _passiveSkillTab = Get<GameObject>((int)GameObjects.PassiveTab);

        BindEvent(_activeToggle.gameObject, OnToggleClicked);
        BindEvent(_passiveToggle.gameObject, OnToggleClicked);
        BindEvent(Get<GameObject>((int)GameObjects.ActivePointInitialBtn), (_) => { OnPointInitialClicked(ESkillType.Active); });
        BindEvent(Get<GameObject>((int)GameObjects.PassivePointInitialBtn), (_) => { OnPointInitialClicked(ESkillType.Passive); });
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdateSkill, RefreshSkillPoint);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdateSkill, RefreshSkillPoint);
    }

    public void Refresh()
    {
        Clear();
        RefreshSkillPoint();
        if (_activeSkills == null || _passiveSkills == null)
        {
            RegisterAllSkillByClassType();
        }

        SetSkillByToggleType();
    }

    private void SetSkillByToggleType()
    {
        if (_activeToggle.isOn)
        {
            _activeSkillTab.SetActive(true);
            int cnt = 0;
            foreach (SkillData skillData in _skillsSortBySlotType.Values)
            {
                if (skillData == null) continue;
                GameObject go = _activeSkillContent.transform.GetChild(cnt).gameObject;
                go.SetActive(true);
                SkillSlot slot = go.GetComponent<SkillSlot>();
                slot.SetInfo(skillData, _slotClickEvent, _slotBeginDragEvent, _slotDragEvent, _skillRegisterEvent);
                cnt++;
            }

            return;
        }

        if (_passiveToggle.isOn)
        {
            _passiveSkillTab.SetActive(true);
            return;
        }
    }

    public void RegisterEvent(Action<SkillData> slotClickEvent, Action<Sprite> slotBeginDragEvent, Action<Vector2> slotDragEvent, Action skillRegisterEvent)
    {
        _slotBeginDragEvent = slotBeginDragEvent;
        _slotDragEvent = slotDragEvent;
        _slotClickEvent = slotClickEvent;
        _skillRegisterEvent = skillRegisterEvent;
    }

    private void RegisterAllSkillByClassType()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        EHeroClassType classType = hero.HeroInfo.LobbyHeroInfo.ClassType;
        List<SkillData> skills = Managers.DataManager.HeroSkillDict.Values.Where(s => s.ClassType == classType).ToList();
        _activeSkills = skills.Where(s => s.SkillType == ESkillType.Active).ToList();
        _passiveSkills = skills.Where(s => s.SkillType == ESkillType.Passive).ToList();

        foreach (SkillData skillData in _activeSkills)
        {
            if (_skillsSortBySlotType.ContainsKey(skillData.SkillSlotType))
            {
                _skillsSortBySlotType[skillData.SkillSlotType] = skillData;
            }
        }
    }

    private void RefreshSkillPoint()
    {
        SkillComponent skillComponent = Utils.GetMySkillComponent();
        if (skillComponent == null)
            return;

        Get<TMP_Text>((int)Texts.ActiveSkillPointTxt).text = skillComponent.ActiveSkillPoint.ToString();
        Get<TMP_Text>((int)Texts.PassiveSkillPointTxt).text = skillComponent.PassiveSkillPoint.ToString();
    }

    private void OnToggleClicked(PointerEventData eventData)
    {
        Refresh();
    }

    private void OnPointInitialClicked(ESkillType skillType)
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        if (Managers.DataManager.CostDict.TryGetValue(hero.HeroData.SkillInitId, out CostData costData) == false)
            return;

        if (hero.CurrencyComponent.CheckEnoughCurrency(costData.CurrencyType, costData.CostValue) == false)
        {
            Managers.UIManager.ShowAlertPopup("Gold가 부족합니다.", Enums.AlertBtnNum.One);
        }
        else
        {
            Managers.UIManager.ShowAlertPopup($"정말로 초기화 하시겠습니까?\n초기화 비용 : {Fomatter.FromatCurrency(costData.CostValue)} Gold", Enums.AlertBtnNum.Two,
            () =>
            {
                SkillComponent skillComponent = Utils.GetMySkillComponent();
                if (skillComponent == null)
                    return;
                skillComponent.SendResetPointPacket(skillType);
            });
        }
    }

    private void Clear()
    {
        _activeSkillTab.SetActive(false);
        _passiveSkillTab.SetActive(false);
    }
}
