using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("StartPlatform");
        transform.position = startingPlatforms[0].transform.position + new Vector3(0, (float)-0.25, 0);
    }
}
