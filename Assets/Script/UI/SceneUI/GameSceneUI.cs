using Google.Protobuf.Struct;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSceneUI : SceneUI
{
    enum GameObjects
    {
        BackBtn,
        InventoryBtn,
        DungeonBtn,
        SettingBtn,
        TestRoomChanger
    }
    enum Sliders
    {
        ExpBar,
        HpBar,
        MpBar
    }
    enum Texts
    {
        AtkTxt,
        DefenceTxt,
        AtkSpeedTxt,
        LevelTxt,
        LevelPercentageTxt,
        HpTxt,
        MpTxt,
        GoldTxt
    }
    Button _toLobbyBtn;

    TMP_Text _atkTxt;
    TMP_Text _defenceTxt;
    TMP_Text _atkSpeedTxt;
    TMP_Text _levelTxt;
    TMP_Text _levelPercentageTxt;
    TMP_Text _hpTxt;
    TMP_Text _mpTxt;
    TMP_Text _goldTxt;

    GameObject _expBar;
    GameObject _hpBar;
    GameObject _mpBar;

    Coroutine _expBarSmoothRoutine;
    protected override void Awake()
    {
        base.Awake();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TMP_Text>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));


        BindEvent(Get<GameObject>((int)GameObjects.BackBtn), OnBackBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.InventoryBtn), OnInventoryBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.DungeonBtn), OnDungeonBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.SettingBtn), OnSettingBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.TestRoomChanger), OnChangeRoomBtnClick);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.ChangeStat, UpdateStat);
        Managers.EventBus.AddEvent(Enums.EventType.ChangeCurrency, UpdateGold);
        Managers.EventBus.AddEvent(Enums.EventType.ChangeGrowth, UpdateGrowth);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeStat, UpdateStat);
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeCurrency, UpdateGold);
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeGrowth, UpdateGrowth);
    }

    public void UpdateStat()
    {
        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;

        StatInfo statInfo = myHero.Stat.StatInfo;

        Get<TMP_Text>((int)Texts.AtkTxt).text = statInfo.AtkDamage.ToString();
        Get<TMP_Text>((int)Texts.DefenceTxt).text = statInfo.Defence.ToString();
        Get<TMP_Text>((int)Texts.AtkSpeedTxt).text = statInfo.AtkSpeed.ToString("N2");
        Get<TMP_Text>((int)Texts.HpTxt).text = $"{statInfo.Hp} / {statInfo.MaxHp}";
        Get<TMP_Text>((int)Texts.MpTxt).text = $"{statInfo.Mp} / {statInfo.MaxMp}";

        Slider hpBar = Get<Slider>((int)Sliders.HpBar);
        hpBar.maxValue = statInfo.MaxHp;
        hpBar.minValue = 0;
        hpBar.value = statInfo.Hp;

        Slider mpBar = Get<Slider>((int)Sliders.MpBar);
        mpBar.maxValue = statInfo.MaxMp;
        mpBar.minValue = 0;
        mpBar.value = statInfo.Mp ;
    }

    public void UpdateGold()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        Get<TMP_Text>((int)Texts.GoldTxt).text = hero.CurrencyComponent.Gold.ToString("N0");
    }

    public void UpdateGrowth()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        GrowthComponent growth = hero.GrowthInfo;

        Get<TMP_Text>((int)Texts.LevelTxt).text = growth.Level.ToString();

        float percentage = ((float)growth.Exp / (float)growth.MaxExp) * 100;
        Get<TMP_Text>((int)Texts.LevelPercentageTxt).text = percentage.ToString("N4");

        Slider expBar = Get<Slider>((int)Sliders.ExpBar);
        expBar.maxValue = growth.MaxExp;
        expBar.minValue = 0;
        if (_expBarSmoothRoutine != null)
            StopCoroutine(_expBarSmoothRoutine);
        _expBarSmoothRoutine = StartCoroutine(CoSmoothChangeBar(expBar, expBar.value, growth.Exp));
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Managers.UIManager.ShowAlertPopup("로비로 이동하시겠습니까?", Enums.AlertBtnNum.Two,
            () => 
            {
                Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
                Managers.GameManager.LeaveGame();
            });
    }

    private void OnChangeRoomBtnClick(PointerEventData eventData)
    {
        Managers.MapManager.ChangeMap(2);
    }

    private void OnInventoryBtnClicked(PointerEventData eventData)
    {
        InventoryUI inventory = Managers.UIManager.ShowPopup<InventoryUI>();
        inventory.Refresh();
    }
    private void OnDungeonBtnClicked(PointerEventData eventData)
    {
        DungeonUI dungeonUI = Managers.UIManager.ShowPopup<DungeonUI>();
    }
    private void OnSettingBtnClicked(PointerEventData eventData)
    {
        //Managers.UIManager.ShowPopup<InventoryUI>();
    }

    IEnumerator CoSmoothChangeBar(Slider bar, float current, int target)
    {
        float duration = 1.0f;
        float process = 0.0f;

        while (process <= duration)
        {
            process += Time.deltaTime;
            bar.value = Mathf.Lerp(current, target, process / duration);

            yield return null;
        }

        bar.value = target;
    }
}
