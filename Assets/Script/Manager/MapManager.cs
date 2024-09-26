using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class MapManager
{
    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinZ { get; set; }
    public int MaxZ { get; set; }
    float[,] _mapCollision;
    public GameObject Map { get; private set; }
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
    public void LoadMap(string mapName)
    {
        DestroyMap();

        GameObject map = Managers.ResourceManager.Instantiate(mapName, Parent);
        map.transform.position = Vector3.zero;
        map.name = $"{mapName}";

        Map = map;

        CoroutineHelper.Instance.StartCoroutine(ReadFile($"Assets/@Resources/Data/Map/{mapName}Data.bin"));
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


    private void DestroyMap()
    {
        if (Map != null)
        {
            Managers.ResourceManager.Destroy(Map);
            Map = null;
        }
    }

    IEnumerator ReadFile(string path)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
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
                yield return null;
            }
        };

    }
}
