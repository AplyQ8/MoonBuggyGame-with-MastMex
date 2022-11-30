using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

    }
    [SerializeField] private GameObject playerSpawner;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
        }
    }
    
    
}
