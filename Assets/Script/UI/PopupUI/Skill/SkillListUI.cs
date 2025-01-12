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
        SkillContent,
        PointInitialBtn,
    }

    enum Texts
    {
        SkillPointTxt,
    }

    GameObject _skillContent;

    Action<SkillData> _slotClickEvent;
    Action _skillRegisterEvent;
    Action<Sprite> _slotBeginDragEvent;
    Action<Vector2> _slotDragEvent;

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TMP_Text>(typeof(Texts));

        _skillContent = Get<GameObject>((int)GameObjects.SkillContent);

        BindEvent(Get<GameObject>((int)GameObjects.PointInitialBtn), (_) => { OnPointInitialClicked(); });
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
        RefreshSkillPoint();
        RefreshSkillInfo();
    }

    private void RefreshSkillInfo()
    {
        SkillComponent skillComponent = Utils.GetMySkillComponent();
        if (skillComponent == null)
            return;
        List<BaseSkill> skills = skillComponent.GetAllSkills();
        int cnt = 0;
        foreach (BaseSkill skill in skills)
        {
            if ((int)skill.SkillData.SkillSlotType <= (int)ESkillSlotType.Dash) continue;

            GameObject go = _skillContent.transform.GetChild(cnt).gameObject;
            go.SetActive(true);
            SkillSlot slot = go.GetComponent<SkillSlot>();
            slot.SetInfo(skill.SkillData, _slotClickEvent, _slotBeginDragEvent, _slotDragEvent, _skillRegisterEvent);
            cnt++;
        }

        return;
    }

    public void RegisterEvent(Action<SkillData> slotClickEvent, Action<Sprite> slotBeginDragEvent, Action<Vector2> slotDragEvent, Action skillRegisterEvent)
    {
        _slotBeginDragEvent = slotBeginDragEvent;
        _slotDragEvent = slotDragEvent;
        _slotClickEvent = slotClickEvent;
        _skillRegisterEvent = skillRegisterEvent;
    }

    private void RefreshSkillPoint()
    {
        SkillComponent skillComponent = Utils.GetMySkillComponent();
        if (skillComponent == null)
            return;

        Get<TMP_Text>((int)Texts.SkillPointTxt).text = skillComponent.SkillPoint.ToString();
    }

    private void OnToggleClicked(PointerEventData eventData)
    {
        Refresh();
    }

    private void OnPointInitialClicked()
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
                skillComponent.SendResetPointPacket();
            });
        }
    }
}
