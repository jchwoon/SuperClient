using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapManager
{
    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinZ { get; set; }
    public int MaxZ { get; set; }
    float[,] _mapCollision;
    public GameObject Map { get; private set; }
    public GameObject LoadedMap { get; private set; }
    public RoomData RoomData { get; private set; }
    public Transform Parent
    {
        get
        {
            GameObject map = GameObject.Find("Map");
            if (map == false)
                map = new GameObject("Map");
            return map.transform;
        }
    }
    private Action _loadedMapAction;
    public void LoadMap(RoomData roomData, Action action = null)
    {
        DestroyMap();
        _loadedMapAction = action;

        string mapName = roomData.MapName;
        GameObject map = Managers.ResourceManager.GetResource<GameObject>(mapName);
        map.name = mapName;
        LoadedMap = map;
        RoomData = roomData;

        TextAsset text = Managers.ResourceManager.GetResource<TextAsset>($"{mapName}MapData");
        Stream s = new MemoryStream(text.bytes);
        ReadFile(new BinaryReader(s));
    }
    public bool CanGo(float z, float x)
    {
        //해당 포인트가 (0.9xx, 0.1xxx)라면 (1, 0)을 좌표를 검사
        int roundZ = (int)MathF.Round(z);
        int roundX = (int)MathF.Round(x);

        if (roundZ < MinZ || roundZ > MaxZ)
            return false;
        if (roundX < MinX || roundX > MaxX)
            return false;

        int applyZ = roundZ - MinZ;
        int applyX = roundX - MinX;
        if (_mapCollision[applyZ, applyX] != 0)
            return false;

        return true;
    }

    public void CreateMap()
    {
        GameObject map = Managers.ResourceManager.Instantiate(LoadedMap, Parent);
        map.transform.position = Vector3.zero;
        Map = map;
    }

    private void DestroyMap()
    {
        if (Map != null)
        {
            Managers.ResourceManager.Destroy(Map);
            Map = null;
            LoadedMap = null;
        }
    }

    public void ChangeMap(int roomId)
    {
        if (Managers.DataManager.RoomDict.TryGetValue(roomId, out RoomData roomData) == false)
            return;
        if (roomId == RoomData.RoomId)
            return;

        Managers.UIManager.ShowFadeUI(fadeTime: 0.5f, isFadeIn: false);
        Managers.ObjectManager.Clear(leaveHero : true);
        LoadMap(roomData);
        CreateMap();
        ChangeRoomToS changeRoomPacket = new ChangeRoomToS();
        changeRoomPacket.RoomId = roomId;
        Managers.NetworkManager.Send(changeRoomPacket);
    }

    private void ReadFile(BinaryReader reader)
    {
        using (reader)
        {
            MinX = reader.ReadInt32();
            MaxX = reader.ReadInt32();
            MinZ = reader.ReadInt32();
            MaxZ = reader.ReadInt32();

            int zCount = MaxZ - MinZ + 1;
            int xCount = MaxX - MinX + 1;
            _mapCollision = new float[zCount, xCount];
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int x = reader.ReadInt32();
                int z = reader.ReadInt32();

                int applyX = x - MinX;
                int applyZ = z - MinZ;

                float height = reader.ReadSingle();
                _mapCollision[applyZ, applyX] = height;
            }
        };

    }
}
