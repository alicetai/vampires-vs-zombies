using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpawn : MonoBehaviour
{
    private float scaleMin = 0.1f;
    private float scaleMax = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeScale();
        RandomizeRotation();
    }

    void RandomizeScale()
    {
        Vector3 randomScale = Vector3.one * Random.Range(scaleMin, scaleMax);
        transform.localScale += randomScale;
        transform.position += new Vector3(0, randomScale.y * 0.5f, 0);
    }

    void RandomizeRotation()
    {
        gameObject.transform.localRotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}
