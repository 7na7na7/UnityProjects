using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject StartEntryPrefab;
    private void Start()
    {
        Instantiate(StartEntryPrefab, transform.position, quaternion.identity);

    }

}
