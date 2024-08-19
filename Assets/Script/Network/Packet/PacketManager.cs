using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PacketManager
{
    private static PacketManager _instance;

    public static PacketManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PacketManager();
            }
            return _instance;
        }
    }


    public enum PacketId
    {
        S_Test = 1,
        C_Test = 2,
    }

    //들어온 패킷 파싱
    Dictionary<ushort, Action<ArraySegment<byte>, ushort>> _parseHandler = new();
    //파싱된 패킷 핸들
    Dictionary<ushort, Action<IMessage>> _handler = new();

    PacketManager()
    {
        PreRaiseHandler();
    }

    private void PreRaiseHandler()
    {
        _parseHandler.Add((ushort)PacketId.S_Test, ParsePacket<S_Test>);
        _handler.Add((ushort)PacketId.S_Test, PacketHandler.S_TestHandler);
    }

    public void ReceivePacket(ArraySegment<byte> segment)
    {
        ushort count = 0;
        ushort packetSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
        count += 2;
        ushort packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += 2;

        Action<ArraySegment<byte>, ushort> action = null;
        if (_parseHandler.TryGetValue(packetId, out action) == true)
        {
            action.Invoke(segment, packetId);
        }
    }

    private void ParsePacket<T>(ArraySegment<byte> segment, ushort id) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(segment.Array, segment.Offset + 4, segment.Count - 4);

        Action<IMessage> action = null;
        if (_handler.TryGetValue(id, out action) == true)
        {
            action.Invoke(packet);
        }
    }
}
