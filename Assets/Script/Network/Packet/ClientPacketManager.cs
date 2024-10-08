
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using ServerCore;

public enum PacketId
{
  ConnectToC = 1,
  ReqHeroListToS = 2,
  ResHeroListToC = 3,
  ReqCreateHeroToS = 4,
  ResCreateHeroToC = 5,
  ReqDeleteHeroToS = 6,
  ResDeleteHeroToC = 7,
  ReqEnterRoomToS = 8,
  ResEnterRoomToC = 9,
  SpawnToC = 10,
  ReqLeaveGameToS = 11,
  MoveToS = 12,
  MoveToC = 13,
  PingCheckToC = 14,
  PingCheckToS = 15,
  DeSpawnToC = 16,
  ReqUseSkillToS = 17,
  ResUseSkillToC = 18,

}


class PacketManager
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

    //들어온 패킷 파싱
    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _parseHandler = new();
    //파싱된 패킷 핸들
    Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new();

    public Action<ushort, IMessage> ClientHandler { get; set; } = null;

    PacketManager()
    {
        PreRaiseHandler();
    }

    private void PreRaiseHandler()
    {
        _parseHandler.Add((ushort)PacketId.ConnectToC, ParsePacket<ConnectToC>);
        _handler.Add((ushort)PacketId.ConnectToC, PacketHandler.ConnectToCHandler);
        _parseHandler.Add((ushort)PacketId.ResHeroListToC, ParsePacket<ResHeroListToC>);
        _handler.Add((ushort)PacketId.ResHeroListToC, PacketHandler.ResHeroListToCHandler);
        _parseHandler.Add((ushort)PacketId.ResCreateHeroToC, ParsePacket<ResCreateHeroToC>);
        _handler.Add((ushort)PacketId.ResCreateHeroToC, PacketHandler.ResCreateHeroToCHandler);
        _parseHandler.Add((ushort)PacketId.ResDeleteHeroToC, ParsePacket<ResDeleteHeroToC>);
        _handler.Add((ushort)PacketId.ResDeleteHeroToC, PacketHandler.ResDeleteHeroToCHandler);
        _parseHandler.Add((ushort)PacketId.ResEnterRoomToC, ParsePacket<ResEnterRoomToC>);
        _handler.Add((ushort)PacketId.ResEnterRoomToC, PacketHandler.ResEnterRoomToCHandler);
        _parseHandler.Add((ushort)PacketId.SpawnToC, ParsePacket<SpawnToC>);
        _handler.Add((ushort)PacketId.SpawnToC, PacketHandler.SpawnToCHandler);
        _parseHandler.Add((ushort)PacketId.MoveToC, ParsePacket<MoveToC>);
        _handler.Add((ushort)PacketId.MoveToC, PacketHandler.MoveToCHandler);
        _parseHandler.Add((ushort)PacketId.PingCheckToC, ParsePacket<PingCheckToC>);
        _handler.Add((ushort)PacketId.PingCheckToC, PacketHandler.PingCheckToCHandler);
        _parseHandler.Add((ushort)PacketId.DeSpawnToC, ParsePacket<DeSpawnToC>);
        _handler.Add((ushort)PacketId.DeSpawnToC, PacketHandler.DeSpawnToCHandler);
        _parseHandler.Add((ushort)PacketId.ResUseSkillToC, ParsePacket<ResUseSkillToC>);
        _handler.Add((ushort)PacketId.ResUseSkillToC, PacketHandler.ResUseSkillToCHandler);
    
    }

    public void ReceivePacket(PacketSession session, ArraySegment<byte> segment)
    {
        ushort count = 0;
        ushort packetSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
        count += 2;
        ushort packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
        if (_parseHandler.TryGetValue(packetId, out action) == true)
        {
            action.Invoke(session, segment, packetId);
        }

    }

    private void ParsePacket<T>(PacketSession session, ArraySegment<byte> segment, ushort id) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(segment.Array, segment.Offset + 4, segment.Count - 4);

        Action<PacketSession, IMessage> action = null;
        if (ClientHandler != null)
        {
            ClientHandler.Invoke(id, packet);
        }
        else if (_handler.TryGetValue(id, out action) == true)
        {
            action.Invoke(session, packet);
        }
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;

        if (_handler.TryGetValue(id, out action) == true)
        {
            return action;
        }

        return null;
    }
}
