using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast: MonoBehaviour
{
    public LayerMask layerMask;
    private readonly int villageLayer = 3;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    // Fire ray at the given position
    public GameObject FireRay(GameObject gameObject, Vector3 position, Quaternion rotation, float maxSlope = 90)
    {
        GameObject spawn = null;
        Ray ray = new Ray(position + Vector3.up * 100, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            // ray hit something!

            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // don't spawn if on a steep slope
            float slope = Vector3.Angle(hit.normal, transform.forward) - 90;
            if (Mathf.Abs(slope) < maxSlope)
            {
                // check for collisions with existing game objects
                if (CheckOverlap(gameObject, hit.point, rotation) == false)
                {
                    // spawn the game object
                    spawn = Instantiate(gameObject, new Vector3(position.x, hit.point.y, position.z), spawnRotation);
                    spawn.transform.localRotation *= rotation;
                    spawn.transform.SetParent(this.transform);
                    spawn.layer = villageLayer;
                }
            }
        }
        return spawn;
    }

    // Check for collisions with existing objects
    public bool CheckOverlap(GameObject gameObject, Vector3 point, Quaternion rotation)
    {
        int numColliders = 0;
        Collider[] colliders = new Collider[1]; // only need to detect one collision
        BoxCollider colliderBox = gameObject.GetComponent<BoxCollider>();
        MeshCollider colliderMesh = gameObject.GetComponent<MeshCollider>();


        if (colliderBox!=null)
        {
            numColliders = Physics.OverlapBoxNonAlloc(point, colliderBox.size, colliders, rotation, villageLayer);
        }
        else if (colliderMesh!=null)
        {
            numColliders = Physics.OverlapBoxNonAlloc(point, colliderMesh.bounds.size, colliders, rotation, villageLayer);
        }
        return numColliders > 0;
    }
}
