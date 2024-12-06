using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
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

    public IInteractable Interactable { get; private set; }
    public event Action<IInteractable> OnInteractableChanged;

    public void SetInteractable(IInteractable interactable)
    {
        OnInteractableChanged?.Invoke(interactable);
        Interactable = interactable;
    }

    public void LeaveGame()
    {
        ReqLeaveGameToS leavePacket = new ReqLeaveGameToS();
        Managers.NetworkManager.Send(leavePacket);
        Managers.Clear();
    }
}
