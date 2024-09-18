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
    }
}
