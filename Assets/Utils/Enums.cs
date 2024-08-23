using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums 
{
    public enum SceneType
    {
        Login,
        Lobby,
        CreateHero,
        Game
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
}
