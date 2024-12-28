using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector
{
    private TerrainData terrainData; 
    private int alphamapWidth; 
    private int alphamapHeight; 
    private float[,,] splatmapData; 
    private int numTextures; 
    public TerrainDetector() 
    { 
        terrainData = Terrain.activeTerrain.terrainData; 
        alphamapWidth = terrainData.alphamapWidth; 
        alphamapHeight = terrainData.alphamapHeight; 
        splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight); 
        numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight); 
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition) 
    { 
        Vector3 splatPosition = new Vector3(); 
        Terrain ter = Terrain.activeTerrain; 
        Vector3 terPosition = ter.transform.position; 
        splatPosition.x = ((worldPosition.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth; 
        splatPosition.z = ((worldPosition.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight; 
        return splatPosition; 
    }

    public int GetActiveTerrainTextureIdx(Vector3 position) 
    { 
        Vector3 terrainCord = ConvertToSplatMapCoordinate(position); 
        int activeTerrainIndex = 0;
        float largestOpacity = 0f; 
        for (int i = 0; i < numTextures; i++) 
        { 
            if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i]) 
            { 
                activeTerrainIndex = i; 
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i]; 
            } 
        } 

        return activeTerrainIndex; 
    }
}
