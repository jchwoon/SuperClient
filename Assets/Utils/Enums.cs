using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums 
{
    public enum SceneType
    {
        Login,
        Lobby,
        Game,
        Loading,
        None
    }

    public enum TouchEvent
    {
        PointerUp,
        PointerDown,
        Click,
        Pressed,
        BeginDrag,
        Drag,
        EndDrag,
        Drop
    }

    public enum AlertBtnNum
    {
        None,
        One,
        Two,
        Three,
    }

    public enum Layers
    {
        Wall = 6,
        Floor
    }

    public enum AnimLayer
    {
        BaseLayer = 0,
        LowerBody = 1,
    }

    public enum EventType
    {
        //Temp
        AtkBtnClick,

        ChangeStat,
        ChangeCurrency,
        ChangeGrowth,
        ChangeHUDInfo,
        UpdateInventory,
        UpdateSkillSet,
        UpdateSkill,
        //해당 던전의 파티 리스트 정보가 업데이트 될 때
        UpdatePartyInfos,
        //내 파티의 멤버가 업데이트 될 때
        UpdatePartyMembers,
        UpdatePartyApplier
    }

    public enum SortingOrderInHUD
    {
        HeroHUD,
        TargetHUD,
    }

    public enum FloatingFontType
    {
        NormalHit,
        CriticalHit,
        Heal,
        Gold,
        Exp
    }

    public enum UseItemFailReason
    {
        None,
        FullHp,
        FullMp,
        Cool
    }

    public enum EConfigIds
    {
        Local = 1,
        Remote = 2,
    }

    public enum EHeroMoveType
    {
        None,
        Joystick,
        Coordinate,
        Target
    }

    public enum ETouchProperty
    {
        Position,
        Delta
    }

    public enum ESoundsType
    {
        BGM,
        BGM2,
        Effect,
        System
    }
}
