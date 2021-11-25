using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    Terrain terrain;
    float terrainWidth;
    float terrainLength;

    Object[] wallPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;

        wallPrefabs = Resources.LoadAll("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Castle walls", typeof(GameObject));
        GenerateWall();
    }

    void GenerateWall()
    {
        GameObject outerWall = (GameObject)wallPrefabs[6];
        Renderer renderer = outerWall.GetComponent<Renderer>();

        Vector2 topLeft = new Vector2(0, 0);
        Vector2 topRight = new Vector2(terrainWidth, 0);
        Vector2 bottomLeft = new Vector2(0, terrainLength);
        Vector2 bottomRight = new Vector2(terrainWidth, terrainLength);

        float groundHeight = Mathf.Min(terrain.terrainData.GetHeight((int)topLeft.x, (int)topLeft.y), terrain.terrainData.GetHeight((int)topRight.x, (int)topRight.y), terrain.terrainData.GetHeight((int)bottomLeft.x, (int)bottomLeft.y), terrain.terrainData.GetHeight((int)bottomRight.x, (int)bottomRight.y));

        SpawnWall(outerWall, renderer.bounds.size, 0, topLeft, topRight, groundHeight);
        SpawnWall(outerWall, renderer.bounds.size, 1, topRight, bottomRight, groundHeight);
        SpawnWall(outerWall, renderer.bounds.size, 2, bottomLeft, bottomRight, groundHeight);
        SpawnWall(outerWall, renderer.bounds.size, 3, topLeft, bottomLeft, groundHeight);
    }

    //
    void SpawnWall(GameObject gameObject, Vector3 size, int direction, Vector2 start, Vector2 end, float groundHeight)
    {
        float width = Mathf.Max(size.x, size.z);
        float totalWidth = Mathf.Max((end - start).x, (end - start).y);
        float deltaX = 0.0f, deltaZ = 0.0f;

        const float MAX_HEIGHT = 20;

        // i specifies the number of walls stacked on top of each other
        for (int i = 0; i < MAX_HEIGHT; i++)
        {
            for (float j = 0; j < totalWidth; j += width)
            {
                if (direction % 2 == 0)
                {
                    deltaX = j;
                }
                else
                {
                    deltaZ = j;
                }

                Vector3 position = new Vector3(start.x + deltaX, groundHeight + i * size.y, start.y + deltaZ);
                GameObject clone = Instantiate(gameObject, position, Quaternion.Euler(0, direction * 90, 0));
                clone.transform.parent = this.transform;
            }
        }
    }
}
