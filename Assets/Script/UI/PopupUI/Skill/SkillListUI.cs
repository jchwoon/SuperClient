using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillListUI : BaseUI
{
    enum GameObjects
    {
        SkillType,
        ActiveSkillGroup,
        ActiveTab,
        PassiveTab
    }

    enum Toggles
    {
        ActiveToggle,
        PassiveToggle
    }

    GameObject _activeSkillGroup;
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

    //Active
    Dictionary<ESkillSlotType, List<SkillData>> _skillsSortBySlotType = new Dictionary<ESkillSlotType, List<SkillData>>()
    {
        { ESkillSlotType.Active1, new List<SkillData>(3) },
        { ESkillSlotType.Active2, new List<SkillData>(3) },
        { ESkillSlotType.Active3, new List<SkillData>(3) },
    };

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));

        _activeToggle = Get<Toggle>((int)Toggles.ActiveToggle);
        _passiveToggle = Get<Toggle>((int)Toggles.PassiveToggle);

        _activeSkillGroup = Get<GameObject>((int)GameObjects.ActiveSkillGroup);
        _activeSkillTab = Get<GameObject>((int)GameObjects.ActiveTab);
        _passiveSkillTab = Get<GameObject>((int)GameObjects.PassiveTab);

        BindEvent(_activeToggle.gameObject, OnToggleClicked);
        BindEvent(_passiveToggle.gameObject, OnToggleClicked);
    }

    public void Refresh()
    {
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
            _passiveSkillTab.SetActive(false);
            int cnt = 0;
            foreach (List<SkillData> skills in _skillsSortBySlotType.Values)
            {
                GameObject go = _activeSkillGroup.transform.GetChild(cnt).gameObject;
                go.SetActive(true);
                SkillSlot[] slots = go.GetComponentsInChildren<SkillSlot>();
                for (int i = 0; i < skills.Count; i++)
                {
                    slots[i].SetInfo(skills[i], _slotClickEvent, _slotBeginDragEvent, _slotDragEvent, _skillRegisterEvent);
                }
                cnt++;
            }

            return;
        }

        if (_passiveToggle.isOn)
        {
            _activeSkillTab.SetActive(false);
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
                _skillsSortBySlotType[skillData.SkillSlotType].Add(skillData);
            }
        }
    }

    private void OnToggleClicked(PointerEventData eventData)
    {
        Refresh();
    }
}
