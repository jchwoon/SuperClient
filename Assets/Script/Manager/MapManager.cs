using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager
{
    Dictionary<(int, int), float> _mapCollision = new Dictionary<(int, int), float>();
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

        //TextAsset txt = Managers.ResourceManager.GetResource<TextAsset>($"{mapName}Data");
        //StringReader reader = new StringReader(txt.text);

        //CoroutineHelper.Instance.StartCoroutine(ReadFile(reader));
    }

    private void DestroyMap()
    {
        if (Map != null)
        {
            Managers.ResourceManager.Destroy(Map);
            Map = null;
        }
    }

    //public bool CanGo(float x, float z)
    //{
    //    float value = 0.0f;
    //    //해당 포인트가 (0.9xx, 0.1xxx)라면 (1, 0)을 좌표를 검사
    //    int roundX = (int)MathF.Round(x);
    //    int roundZ = (int)MathF.Round(z);
    //    if (_mapCollision.TryGetValue((roundX, roundZ), out value) == false)
    //        return false;

    //    if (value == -1)
    //        return false;

    //    return true;
    //}

    //IEnumerator ReadFile(StringReader reader)
    //{
    //    while (true)
    //    {
    //        string line = reader.ReadLine();
    //        if (string.IsNullOrEmpty(line) == true)
    //            break;

    //        string[] pos = line.Split(',');

    //        int posX = int.Parse(pos[0]);
    //        int posZ = int.Parse(pos[1]);
    //        float posY = float.Parse(pos[2]);

    //        _mapCollision.Add((posX, posZ), posY);
    //        yield return null;
    //    }    
    //}
}
