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
    public string Name { get; protected set; }

    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }
    protected virtual void OnDisable()
    {

    }
    protected virtual void Start()
    {
        if (isMachineInit == false)
            Machine = new StateMachine();
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
    protected virtual void SetObjInfo(ObjectInfo info)
    {
        ObjectId = info.ObjectId;
        ObjectType = info.ObjectType;
    }

    protected virtual void OnRevival()
    {
        Machine.OnRevival();
    }

    public virtual void OnContactMyHero()
    {

    }
    public virtual void OnDetactMyHero()
    {

    }

    #region Network Send
    #endregion

    #region Network Receive
    public void ReceivePosInfo(MoveToC movePacket)
    {
        if (Machine == null)
            return;

        Machine.UpdatePosInput(movePacket);
    }
    public void HandleTeleport(TeleportToC telpoPacket)
    {
        Vector3 telpoPos = new Vector3(telpoPacket.PosInfo.PosX, telpoPacket.PosInfo.PosY, telpoPacket.PosInfo.PosZ);
        gameObject.transform.position = telpoPos;

        if (telpoPacket.TelpoType == ETeleportType.Respawn)
        {
            OnRevival();
        }
    }
    #endregion
}
