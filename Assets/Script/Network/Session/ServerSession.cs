using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSession : PacketSession
{
    public ArraySegment<byte> Send(IMessage packet)
    {
        PacketId packetId = (PacketId)Enum.Parse(typeof(PacketId), packet.Descriptor.Name);
        ushort size = (ushort)packet.CalculateSize();
        ArraySegment<byte> sendBuffer = new ArraySegment<byte>(new byte[size + 4], 0, size + 4);
        //길이가 2인 바이트 배열의 0번째부터 sizeof(ushort)까지 sendBuffer에 복사
        ushort count = 0;
        Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer.Array, sendBuffer.Offset, sizeof(ushort));
        count += 2;
        Array.Copy(BitConverter.GetBytes((ushort)packetId), 0, sendBuffer.Array, sendBuffer.Offset + count, sizeof(ushort));
        count += 2;
        Array.Copy(packet.ToByteArray(), 0, sendBuffer.Array, sendBuffer.Offset + count, size);
        return sendBuffer;
    }
    public override void OnConnected()
    {
        Debug.Log("hhh");
    }

    public override void OnDisconnected()
    {
    }

    public override void OnRecvPacket(ArraySegment<byte> segment)
    {
        Debug.Log("heloo");
        PacketManager.Instance.ReceivePacket(segment);
    }

    public override void OnSend()
    {

    }
}
