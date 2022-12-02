using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject statusImage;
    [SerializeField] private Sprite checkMark;
    [SerializeField] private GameObject buggy;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject wall;
    private string _id;
    private bool _status;

    public void SetID(string id)
    {
        _id = id;
    }

    public bool CheckID(string id) => _id == id;

    public void ChangeStatus()
    { 
        _status = !_status;
        statusImage.GetComponent<Image>().sprite = checkMark;
    }

    public Vector3 ReturnSpawnPos()
    {
        return spawnPoint.transform.position;
    }

    public Vector3 ReturnWallPos()
    {
        return wall.transform.position;
    }

    public float ReturnEnemyXPos()
    {
        return gameObject.transform.position.x;
    }

    public float ReturnEnemyYPos()
    {
        return gameObject.transform.position.y;
    }


}
