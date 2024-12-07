using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Data;

public class CreatureHUD : BaseUI
{
    enum Sliders
    {
        HpBar,
        MpBar
    }
    enum Texts
    {
        NameTxt
    }
    enum GameObjects
    {
        TargetMark,
        Bar
    }

    Creature _owner;

    protected override void Awake()
    {
        base.Awake();
        Bind<Slider>(typeof(Sliders));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.ChangeHUDInfo, RefreshHUD);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeHUDInfo, RefreshHUD);
    }

    protected void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }

    public void AddHUD(Creature owner)
    {
        gameObject.SetActive(true);
        _owner = owner;
        RefreshHUD();
    }

    public void RemoveHUD()
    {
        gameObject.SetActive(false);
    }
  
    private void RefreshHUD()
    {
        Init();
        SetName();
        SetBar();
    }

    private void Init()
    {
        Get<Slider>((int)Sliders.HpBar).gameObject.SetActive(true);
        Get<Slider>((int)Sliders.MpBar).gameObject.SetActive(true);
        Get<GameObject>((int)GameObjects.TargetMark).SetActive(true);
        Get<GameObject>((int)GameObjects.Bar).SetActive(true);
    }

    public void SetBar()
    {
        switch (_owner.ObjectType)
        {
            case EObjectType.Hero:
                SetHeroBar();
                break;
            case EObjectType.Monster:
                SetMonsterBar();
                break;
            default:
                break;
        }
    }

    private void SetHeroBar()
    {
        if (_owner.ObjectId != Managers.ObjectManager.MyHero.ObjectId)
            Get<GameObject>((int)GameObjects.Bar).SetActive(false);
        gameObject.GetComponent<Canvas>().sortingOrder = (int)Enums.SortingOrderInHUD.HeroHUD;

        StatInfo statInfo = _owner.Stat.StatInfo;
        float hpValue = (float)statInfo.Hp / (float)statInfo.MaxHp;
        float mpValue = (float)statInfo.Mp / (float)statInfo.MaxMp;

        Get<Slider>((int)Sliders.HpBar).value = hpValue;
        Get<Slider>((int)Sliders.MpBar).value = mpValue;
    }
    private void SetMonsterBar()
    {
        if (_owner.IsTargetted == false)
            Get<GameObject>((int)GameObjects.TargetMark).SetActive(false);
        Get<Slider>((int)Sliders.MpBar).gameObject.SetActive(false);
        gameObject.GetComponent<Canvas>().sortingOrder = (int)Enums.SortingOrderInHUD.TargetHUD;

        StatInfo statInfo = _owner.Stat.StatInfo;
        float hpValue = (float)statInfo.Hp / (float)statInfo.MaxHp;

        Get<Slider>((int)Sliders.HpBar).value = hpValue;
    }
    private void SetName()
    {
        Get<TMP_Text>((int)Texts.NameTxt).text = _owner.Name;
    }
}
