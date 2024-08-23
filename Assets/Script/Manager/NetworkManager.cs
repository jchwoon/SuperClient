using Google.Protobuf;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager
{
    Session _session = null;
    public Action OnConnectedAction { get; set; }
    public Action OnFailedAction { get; set; }
    public void Connect()
    {
        _session = new ServerSession();
        Connector connector = new Connector();
        IPAddress ipAdress = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAdress, 4000);

        connector.OnConnectedAction = OnConnectedAction;
        connector.OnFailedAction = OnFailedAction;
        connector.Connect(endPoint, () => { return _session; });
    }

    public void Send(ArraySegment<byte> segment)
    {
        if (_session != null )
            _session.Send(segment);
    }

    public void Disconnect()
    {
        if (_session != null)
            _session.CloseSocket();

        _session = null;
    }

    public void Update()
    {
        ProcessCallback();
        ProcessPacket();
    }

    private void ProcessPacket()
    {
        if (_session != null)
        {
            List<Packet> packetList = PacketQueue.Instance.Pop();

            foreach (Packet packet in packetList)
            {
                Action<IMessage> action = PacketManager.Instance.GetPacketHandler(packet.id);
                action?.Invoke(packet.packet);
            }
        }
    }

    private void ProcessCallback()
    {
        List<Action> callbackList = CallbackQueue.Instance.Pop();

        foreach (Action action in callbackList)
        {
            action?.Invoke();
        }
    }
}
