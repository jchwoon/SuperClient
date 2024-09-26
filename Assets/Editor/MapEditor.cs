using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR
    [MenuItem("Tools/GenerateMap %#g")]
    private static void CastRays()
    {
        int mapSizeX = 80;
        int mapSizeZ = 100;
        int gridSpacing = 1;
        GameObject go = GameObject.Find("MapTool");
        Terrain terrain = go.GetComponent<Terrain>();
        Vector3 terrainPosition = terrain.transform.position;


        using (var writer = new BinaryWriter(File.Open("Assets/@Resources/Data/Map/map.bin", FileMode.Create)))
        {
            writer.Write((int)terrainPosition.x);//置社 x
            writer.Write(mapSizeX + (int)terrainPosition.x);//置企 x
            writer.Write((int)terrainPosition.z);//置社 z
            writer.Write(mapSizeZ + (int)terrainPosition.z);//置企 z
            for (int x = 0; x <= mapSizeX; x++)
            {
                for (int z = 0; z <= mapSizeZ; z++)
                {
                    int posX = (int)terrainPosition.x + x * gridSpacing;
                    int posZ = (int)terrainPosition.z + z * gridSpacing;
                    Vector3 rayStartPos = new Vector3(posX, terrainPosition.y, posZ);


                    int mask = LayerMask.GetMask("CanMove") | LayerMask.GetMask("Wall");
                    
                    if (Physics.Raycast(rayStartPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, mask))
                    {

                        if (hit.point.y <= 0.4f) continue;
                        writer.Write(posX);
                        writer.Write(posZ);
                        writer.Write(hit.point.y);
                    }
                }
            }
        }
    }
#endif
}
