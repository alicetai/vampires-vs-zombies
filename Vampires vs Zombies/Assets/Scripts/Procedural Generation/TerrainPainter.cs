using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPainter : MonoBehaviour
{
    public const float NOISE_AMOUNT = 0.02f;
    private TerrainData terrainData;
    private float[] splats; // array of alpha values for each texture

    // Class used to store and determine alpha values for textures
    [System.Serializable]
    public class SplatHeights
    {
        public int textureIndex; // index in the texture list
        public int minHeight; // min height on the terrain
        public int overlapDistance;
    }

    public SplatHeights[] splatHeights;

    // Normalise by summing all the values in the array and dividing each by the result
    private void Normalize(float[] values)
    {
        float total = 0;
        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
        }
        for (int i = 0; i < values.Length; i++)
        {
            values[i] /= total;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        splats = new float[splatHeights.Length];

        PaintTerrain();
    }

    void PaintTerrain()
    {
        float[,,] splatMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        // loop through the terrain, one pixel at a time
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float terrainHeight = terrainData.GetHeight(y, x); // height at the position (x, y)

                // loop through the splat values
                for (int i = 0; i < splatHeights.Length; i++)
                {
                    float startHeight = splatHeights[i].minHeight * Mathf.PerlinNoise(x * NOISE_AMOUNT, y * NOISE_AMOUNT) - splatHeights[i].overlapDistance * Mathf.PerlinNoise(x * NOISE_AMOUNT, y * NOISE_AMOUNT);
                    float endHeight = 0;

                    if (i != splatHeights.Length - 1)
                    {
                        // not the last height in the list
                        endHeight = splatHeights[i + 1].minHeight * Mathf.PerlinNoise(x * NOISE_AMOUNT, y * NOISE_AMOUNT) + splatHeights[i + 1].overlapDistance * Mathf.PerlinNoise(x * NOISE_AMOUNT, y * NOISE_AMOUNT);
                    }

                    if (i == splatHeights.Length - 1 && (terrainHeight >= startHeight))
                    {
                        // last height in the list
                        splats[i] = 1;
                    }
                    else if ((terrainHeight >= startHeight) && (terrainHeight <= endHeight))
                    {
                        // set the splat opacity to full for the current pixel
                        splats[i] = 1;
                    }
                }

                Normalize(splats);
                // update the splat map data with the new values
                for (int j = 0; j < splatHeights.Length; j++)
                {
                    splatMap[x, y, j] = splats[j];
                }
            }
        }

        // update the terrain data
        terrainData.SetAlphamaps(0, 0, splatMap);
    }
}
