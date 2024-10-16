using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour 
{
    protected bool isMachineInit = false;
    public int ObjectId { get; set; }
    public EObjectType ObjectType { get; set; }
    public virtual StateMachine Machine { get; set; }

    protected virtual void Awake()
    {
        if (isMachineInit == false)
            Machine = new StateMachine();
    }

    protected virtual void OnEnable()
    {

    }
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (Machine == null)
            return;
        Machine.Update();
    }

    protected virtual void SetPos(GameObject go, PosInfo posInfo)
    {
        go.transform.position = new Vector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ);
        go.transform.eulerAngles = new Vector3(0, posInfo.RotY, 0);
    }
    protected virtual void SetInfo(CreatureInfo info)
    {
        ObjectId = info.ObjectInfo.ObjectId;
        ObjectType = info.ObjectInfo.ObjectType;
    }

    #region Network Send
    #endregion

    #region Network Receive
    public void ReceivePosInfo(MoveToC movePacket)
    {
        Machine.UpdatePosInput(movePacket.PosInfo);
    }
    #endregion
}
