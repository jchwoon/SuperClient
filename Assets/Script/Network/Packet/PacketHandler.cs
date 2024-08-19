using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PacketHandler
{
    public static void S_TestHandler(IMessage packet)
    {
        S_Test test = packet as S_Test;

        Debug.Log(test.Name);
    }
}
