using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assets from: 
// https://assetstore.unity.com/packages/essentials/tutorial-projects/viking-village-urp-29140
// https://assetstore.unity.com/packages/3d/environments/fantasy/mega-fantasy-props-pack-87811 


public class VillageGenerator : MonoBehaviour
{
    Terrain terrain;
    float terrainWidth;
    float terrainLength;
    public LayerMask layer;
    public RayCast rayCaster;

    const int MAX_SEED = 100;
    [Range(0, MAX_SEED)] public int seed;
    public bool randomizeSeed;

    Dictionary<Vector2, Block> blocks = new Dictionary<Vector2, Block>();

    Object[] housePrefabs;
    Object[] barrelPrefabs;
    Object[] rockPrefabs;
    Object[] firewoodPrefabs;
    GameObject grassPrefab;
    GameObject fencePrefab;
    GameObject wellPrefab;
    GameObject wagonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // get the terrain data
        terrain = Terrain.activeTerrain;
        terrainWidth = terrain.terrainData.size.x;
        terrainLength = terrain.terrainData.size.z;

        rayCaster = gameObject.AddComponent<RayCast>();

        LoadPrefabs();

        GenerateBlocks();
        GenerateVillage();
        GenerateRocks();
    }

    System.Random GetRandom()
    {
        if (randomizeSeed)
        {
            seed = Random.Range(0, MAX_SEED);
        }
        return new System.Random(seed.GetHashCode());
    }

    void LoadPrefabs()
    {
        housePrefabs = Resources.LoadAll("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Houses", typeof(GameObject));
        barrelPrefabs = Resources.LoadAll("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Miscellaneous/Barrels", typeof(GameObject));
        fencePrefab = Resources.Load("Procedural Generation/Viking Village/Prefabs/Props/Fences/Fence") as GameObject;
        rockPrefabs = Resources.LoadAll("Procedural Generation/Free Rocks/_prefabs", typeof(GameObject));
        grassPrefab = Resources.Load("Procedural Generation/NatureStarterKit2/Materials/Grass") as GameObject;
        firewoodPrefabs = Resources.LoadAll("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Miscellaneous/Firewood", typeof(GameObject));
        wagonPrefab = Resources.Load("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Miscellaneous/Wagon") as GameObject;
        wellPrefab = Resources.Load("Procedural Generation/Mega Fantasy Props Pack/Prefabs/Miscellaneous/Wells/well") as GameObject;
    }

    void GenerateHouse(Vector3 position, Quaternion rotation)
    {
        if (rayCaster.FireRay((GameObject)housePrefabs[Random.Range(0, housePrefabs.Length)], position, rotation, 0.5f)!=null)
        {
            if (Random.Range(0, 100) < 25)
            {
                // generate a fence, but only if the slope gradient is less than 5
                rayCaster.FireRay(fencePrefab, position, rotation, 1.0f);
            }
        }

        // and spawn barrels beside it
        var barrelPosition = Random.Range(1, 4) switch
        {
            1 => position + Vector3.left * 10,// left
            2 => position + Vector3.right * 10,// right
            3 => position + Vector3.forward * 10,// front
            4 => position + Vector3.back * 10,// back
            _ => position,
        };
        rayCaster.FireRay((GameObject)barrelPrefabs[Random.Range(0, barrelPrefabs.Length)], barrelPosition, rotation);
    }

    // Procedural generation algorithm, based off L-systems

    // Reading material:
    // Stack Exchange, ?Using L-Systems to Procedurally Generate Cities? https://gamedev.stackexchange.com/questions/86234/using-l-systems-to-procedurally-generate-cities
    // Tobias Mansfield-Williams, ?Procedural City Generation? https://www.tmwhere.com/city_generation.html

    void GenerateBlocks()
    {
        Stack<Block> newBlocks = new Stack<Block>(); // new blocks/segments ready to be verified

        // start the village as one large block
        Block village = new Block(new Rect(200, 200, 600, 600)); // village dimensions
        Block world = new Block(new Rect(0, 0, terrainWidth, terrainLength));

        // add it to the blocks list
        newBlocks.Push(village);

        // iterate through the existing village blocks
        Block current;
        while (newBlocks.Count > 0)
        {
            current = newBlocks.Pop();

            // verify if the current block fulfils constraints
            if (current.IsIn(world))
            {
                // minimum length and width of the block
                if (current.Rectangle.width > Block.MaxSize && current.Rectangle.height > Block.MaxSize)
                {
                    // sub-blocks will overwrite their parent block by list key
                    // in the end, the list will only contain children blocks
                    blocks[current.Rectangle.position] = current;

                    // divide the rectangle block into two
                    Block[] children = current.Grow();
                    foreach (Block b in children)
                    {
                        newBlocks.Push(b);
                    }
                }
            }
        }
    }

    // Spawn village game objects on the grid blocks produced
    void GenerateVillage()
    {
        float maxHeight = 20.0f;

        foreach (KeyValuePair<Vector2, Block> block in blocks)
        {
            Vector2 topLeft = block.Value.Rectangle.position;
            Vector3 topLeftPosition = new Vector3(topLeft.x, 0, topLeft.y);
            Vector2 center = block.Value.Rectangle.center;
            Vector3 centerPosition = new Vector3(center.x, 0, center.y);

            Rect rectangle = block.Value.Rectangle;

            // randomly generate grass
           // GenerateGrass(rectangle);

            // place houses if terrain height is below threshold
            if (terrain.SampleHeight(new Vector3(center.x, 0, center.y)) < maxHeight)
            {
                // generate a random rotation at a right angle
                int direction = Random.Range(0, 3);
                Quaternion rotation = Quaternion.Euler(0, direction * 90, 0);

                // generate house in the middle
                GenerateHouse(centerPosition, rotation);

                if (Random.Range(0, 100) < 50)
                {
                    rayCaster.FireRay(wagonPrefab, new Vector3(Random.Range(rectangle.xMin, rectangle.xMax), 0, Random.Range(rectangle.yMin, rectangle.yMax)), rotation);
                }
                rayCaster.FireRay((GameObject)firewoodPrefabs[Random.Range(0, firewoodPrefabs.Length)], topLeftPosition, rotation);
                //rayCaster.FireRay(wagonPrefab, new Vector3(Random.Range(rectangle.xMin, rectangle.xMax), 0, Random.Range(rectangle.yMin, rectangle.yMax)), rotation);
                //rayCaster.FireRay(wellPrefab, new Vector3(Random.Range(rectangle.xMin, rectangle.xMax), 0, Random.Range(rectangle.yMin, rectangle.yMax)), rotation);
            }
        }
        // randomly generate grass
        //GenerateGrass(new Rect(0, 0, terrainWidth, terrainLength));
    }

    // void GenerateGrass(Rect patch)
    // {
    //     // sample size based on patch area
    //     float area = patch.width * patch.height;
    //     float density = 0.2f;
    //     int count = Mathf.FloorToInt(area * density);

    //     float minHeight = 0.5f;
    //     float maxHeight = 8.0f;

    //     for (int i = 0; i < count; i++)
    //     {
    //         // generate random position
    //         Vector3 position = new Vector3(Random.Range(patch.xMin, patch.xMax), 0, Random.Range(patch.yMin, patch.yMax));

    //         if (terrain.SampleHeight(position) > minHeight && terrain.SampleHeight(position) < maxHeight)
    //         {
    //             rayCaster.FireRay(grassPrefab, position, Quaternion.identity, 3.0f);
    //         }
    //     }
    // }

    void GenerateRocks()
    {
        GameObject rock;
        int count = 200;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(Random.Range(0, terrainWidth), 0, Random.Range(0, terrainLength));
            rock = rayCaster.FireRay((GameObject)rockPrefabs[Random.Range(0, rockPrefabs.Length)], position, Quaternion.identity);
            if (rock!=null) {
                rock.AddComponent<RandomizeSpawn>();
            }
        }
    }

}