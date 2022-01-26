using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
[ExecuteInEditMode]
public class TerrainNoiser : MonoBehaviour
{
    [SerializeField] private float scale = 20f;

    private Terrain terrain;

    public void GenerateTerrain()
    {
        if(terrain == null) { terrain = this.GetComponent<Terrain>(); }
        terrain.terrainData.heightmapResolution = (int)terrain.terrainData.size.x + 1;
        terrain.terrainData.SetHeights(0, 0, GenerateHeights());
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[(int)terrain.terrainData.size.x, (int)terrain.terrainData.size.z];
        for(int x = 0; x < terrain.terrainData.size.x; x++)
        {
            for(int z = 0; z < terrain.terrainData.size.z; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
        }
        return heights;
    }

    float CalculateHeight(float x, float z)
    {
        float xCoord = (x + this.transform.position.z) / (float)terrain.terrainData.size.x * scale;
        float zCoord = (z + this.transform.position.x) / (float)terrain.terrainData.size.z * scale;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }
}
