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
        Monster = 8,
        DropItem
    }

    public enum AnimLayer
    {
        BaseLayer = 0,
        LowerBody = 1,
    }

    public enum EventType
    {
        None,
        //Temp
        AtkBtnClick,
        //Temp
        DashBtnClick,
        ChangeStat,
        ChangeCurrency,
        ChangeGrowth,
        ChangeHUDInfo,
        PickUpBtnClick,
        UpdateInventory
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
}
