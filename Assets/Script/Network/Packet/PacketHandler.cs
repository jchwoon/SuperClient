using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PacketHandler
{
    public static void ConnectToCHandler(IMessage packet)
    {
        ConnectToC connectPacket = (ConnectToC)packet;
        Debug.Log("Sever���� ���� �Ͱ�");
    }

    public static void LoginToCHandler(IMessage packet)
    {

    }
}
