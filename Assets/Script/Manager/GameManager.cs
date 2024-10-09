using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int SelectHeroIdx { get; set; }

    private Vector2 _moveInput = Vector2.zero;
    public event Action<Vector2> OnJoystickChanged;

    public Vector2 MoveInput
    {
        get { return _moveInput; }
        set
        {
            _moveInput = value;
            OnJoystickChanged?.Invoke(MoveInput);
        }
    }
}
