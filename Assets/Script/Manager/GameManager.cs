using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int SelectHeroIdx { get; set; }

    private Vector2 _moveInput = Vector2.zero;
    //private EMoveDir _moveDir = EMoveDir.None;
    //private EMoveForce _moveForce = EMoveForce.None;

    public Vector2 MoveInput
    {
        get { return _moveInput; }
        set
        {
            _moveInput = value;
            OnJoystickChanged?.Invoke(MoveInput);
        }
    }
    //public EMoveDir MoveDir
    //{
    //    get { return _moveDir; }
    //    set
    //    {
    //        if (_moveDir != value)
    //        {
    //            _moveDir = value;
    //            Debug.Log(value);
    //            OnMoveOperationChanged?.Invoke(MoveDir, MoveForce);
    //        }
    //    }
    //}
    //public EMoveForce MoveForce
    //{
    //    get { return _moveForce; }
    //    set
    //    {
    //        if (_moveForce != value)
    //        {
    //            _moveForce = value;
    //            OnMoveOperationChanged?.Invoke(MoveDir, MoveForce);
    //        }
    //    }
    //}

    public event Action<Vector2> OnJoystickChanged;
    //public event Action<EMoveDir, EMoveForce> OnMoveOperationChanged;
}
