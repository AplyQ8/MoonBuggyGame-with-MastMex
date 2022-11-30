using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject hole;
    [SerializeField] private GameObject longRock;
    [SerializeField] private GameObject rock;

    public void SpawnBarrier(Vector3 spawnPos, Vector3 target, string type, float speed)
    {
        switch (type)
        {
            case "hole":
                var holeBarrier = Instantiate(hole, spawnPos, Quaternion.identity);
                holeBarrier.GetComponent<BarrierScript>().SetInfo(speed, target);
                Debug.Log($"Hole has been spawned at {DateTime.Now}");
                break;
            case "block":
                var blockBarrier = Instantiate(rock, spawnPos, Quaternion.identity);
                blockBarrier.GetComponent<BarrierScript>().SetInfo(speed, target);
                Debug.Log($"Block has been spawned at {DateTime.Now}");
                break;
            case "long_block":
                var longblockBarrier = Instantiate(longRock, spawnPos, Quaternion.identity);
                longblockBarrier.GetComponent<BarrierScript>().SetInfo(speed, target);
                Debug.Log($"Long block has been spawned at {DateTime.Now}");
                break;
        }
    }
    
}
