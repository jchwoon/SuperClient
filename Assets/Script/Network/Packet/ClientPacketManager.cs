
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
  PreEnterRoomToS = 8,
  PreEnterRoomToC = 9,
  ReqEnterRoomToS = 10,
  ResEnterRoomToC = 11,
  SpawnToC = 12,
  ReqLeaveGameToS = 13,
  MoveToS = 14,
  MoveToC = 15,
  PingCheckToC = 16,
  PingCheckToS = 17,
  DeSpawnToC = 18,
  ReqUseSkillToS = 19,
  ResUseSkillToC = 20,
  ModifyStatToC = 21,
  ModifyOneStatToC = 22,
  DieToC = 23,
  TeleportToC = 24,
  RewardToC = 25,
  PickupDropItemToS = 26,
  PickupDropItemToC = 27,
  AddItemToC = 28,
  UseItemToS = 29,
  UseItemToC = 30,
  EquipItemToS = 31,
  UnEquipItemToS = 32,
  ChangeSlotTypeToC = 33,

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
        _parseHandler.Add((ushort)PacketId.PreEnterRoomToC, ParsePacket<PreEnterRoomToC>);
        _handler.Add((ushort)PacketId.PreEnterRoomToC, PacketHandler.PreEnterRoomToCHandler);
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
        _parseHandler.Add((ushort)PacketId.ModifyStatToC, ParsePacket<ModifyStatToC>);
        _handler.Add((ushort)PacketId.ModifyStatToC, PacketHandler.ModifyStatToCHandler);
        _parseHandler.Add((ushort)PacketId.ModifyOneStatToC, ParsePacket<ModifyOneStatToC>);
        _handler.Add((ushort)PacketId.ModifyOneStatToC, PacketHandler.ModifyOneStatToCHandler);
        _parseHandler.Add((ushort)PacketId.DieToC, ParsePacket<DieToC>);
        _handler.Add((ushort)PacketId.DieToC, PacketHandler.DieToCHandler);
        _parseHandler.Add((ushort)PacketId.TeleportToC, ParsePacket<TeleportToC>);
        _handler.Add((ushort)PacketId.TeleportToC, PacketHandler.TeleportToCHandler);
        _parseHandler.Add((ushort)PacketId.RewardToC, ParsePacket<RewardToC>);
        _handler.Add((ushort)PacketId.RewardToC, PacketHandler.RewardToCHandler);
        _parseHandler.Add((ushort)PacketId.PickupDropItemToC, ParsePacket<PickupDropItemToC>);
        _handler.Add((ushort)PacketId.PickupDropItemToC, PacketHandler.PickupDropItemToCHandler);
        _parseHandler.Add((ushort)PacketId.AddItemToC, ParsePacket<AddItemToC>);
        _handler.Add((ushort)PacketId.AddItemToC, PacketHandler.AddItemToCHandler);
        _parseHandler.Add((ushort)PacketId.UseItemToC, ParsePacket<UseItemToC>);
        _handler.Add((ushort)PacketId.UseItemToC, PacketHandler.UseItemToCHandler);
        _parseHandler.Add((ushort)PacketId.ChangeSlotTypeToC, ParsePacket<ChangeSlotTypeToC>);
        _handler.Add((ushort)PacketId.ChangeSlotTypeToC, PacketHandler.ChangeSlotTypeToCHandler);
    
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
