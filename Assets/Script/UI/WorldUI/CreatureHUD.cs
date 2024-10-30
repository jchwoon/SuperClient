using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;

public class CreatureHUD : BaseUI
{
    enum Sliders
    {
        HpBar,
        MpBar
    }
    enum Texts
    {
        NameTxt,
        LevelTxt
    }
    enum GameObjects
    {
        TargetMark,
        Level,
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
        Managers.EventBus.AddEvent(Enums.EventType.ChangeHUDInfo, SetBar);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeHUDInfo, SetBar);
    }

    protected void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;
    }

    public void AddHUD(Creature owner)
    {
        gameObject.SetActive(true);
        _owner = owner;
        SetName();
        SetBar();
        SetLevel();
    }

    public void RemoveHUD()
    {
        gameObject.SetActive(false);
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

    private void SetLevel()
    {
        if (_owner.ObjectType == EObjectType.Hero)
        {
            Hero hero = (Hero)_owner;
            Get<TMP_Text>((int)Texts.LevelTxt).text = hero.Level.ToString();
        }
    }
    private void SetName()
    {
        Get<TMP_Text>((int)Texts.NameTxt).text = _owner.Name;
    }

    private void SetHeroBar()
    {
        Get<GameObject>((int)GameObjects.TargetMark).SetActive(false);
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
        Get<Slider>((int)Sliders.MpBar).gameObject.SetActive(false);
        Get<GameObject>((int)GameObjects.Level).SetActive(false);
        gameObject.GetComponent<Canvas>().sortingOrder = (int)Enums.SortingOrderInHUD.TargetHUD;

        StatInfo statInfo = _owner.Stat.StatInfo;
        float hpValue = (float)statInfo.Hp / (float)statInfo.MaxHp;

        Get<Slider>((int)Sliders.HpBar).value = hpValue;
    }
}
