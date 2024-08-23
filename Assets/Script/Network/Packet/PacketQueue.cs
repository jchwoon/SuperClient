using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Packet
{
    public ushort id;
    public IMessage packet;
}

public class PacketQueue
{
    private static PacketQueue _instance;
    public static PacketQueue Instance { 
        get
        {
            if (_instance == null )
                _instance = new PacketQueue();

            return _instance;
        }
    }

    Queue<Packet> _packetQueue = new Queue<Packet>();
    object _lock = new object();

    public void Push(ushort id, IMessage packet)
    {
        lock( _lock )
        {
            _packetQueue.Enqueue(new Packet() { id = id, packet = packet });
        }
    }

    public List<Packet> Pop()
    {
        List<Packet> packetList = new();
        lock( _lock )
        {
            while ( _packetQueue.Count > 0 )
            {
                Packet packet = _packetQueue.Dequeue();
                packetList.Add(packet);
            }
        }

        return packetList;
    }
}
