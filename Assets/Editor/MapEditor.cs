using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class MapEditor
{
    [MenuItem("Tools/GenerateMap %#g")]
    private static void CastRays()
    {
        int mapSizeX = 80;
        int mapSizeZ = 100;
        float gridSpacing = 1f;
        GameObject go = GameObject.Find("MapTool");
        Terrain terrain = go.GetComponent<Terrain>();
        Vector3 terrainPosition = terrain.transform.position;

        using (var writer = new StreamWriter("Assets/@Resources/Data/Map/map.txt"))
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                for (int z = 0; z <= mapSizeZ; z++)
                {
                    float posX = terrainPosition.x + x * gridSpacing;
                    float posZ = terrainPosition.z + z * gridSpacing;
                    Vector3 rayStartPos = new Vector3(posX, terrainPosition.y, posZ);


                    int mask = LayerMask.GetMask("CanMove") | LayerMask.GetMask("Wall");
                    
                    if (Physics.Raycast(rayStartPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, mask))
                    {
                        int hitLayer = hit.collider.gameObject.layer;

                        if (hitLayer == LayerMask.NameToLayer("CanMove"))
                        {
                            writer.WriteLine($"{posX},{posZ},{hit.point.y}");
                        }
                    }
                }
            }
        }


    }

}
