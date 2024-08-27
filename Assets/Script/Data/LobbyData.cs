using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyData : MonoBehaviour
{
    public List<LobbyHeroInfo> LobbyHeroInfos { get; private set; } = new List<LobbyHeroInfo>();


    public void SetHeroInfo(List<LobbyHeroInfo> heroeInfos)
    {
        LobbyHeroInfos = heroeInfos;
    }
}
