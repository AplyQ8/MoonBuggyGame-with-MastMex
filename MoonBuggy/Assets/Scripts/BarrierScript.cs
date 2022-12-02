using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BarrierScript : MonoBehaviour
{
    [SerializeField] private GameObject objectsToSpawn;
    private float _speed;
    private float _yPos;
    private Vector3 _target;

    public void SetInfo(float speed, Vector3 target)
    {
        _speed = speed;
        _target = target;
    }

    
    private void Move()
    {
        //Vector3 dir = new Vector3(73, _yPos);
        Vector3 dir = transform.right * (-1);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, _speed * Time.deltaTime);
        if(transform.position.x <= -11) Destroy(gameObject);

    }

    void Update()
    {
        //_speed += 0.2f;
        Move();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Wall")
        {
            Destroy(gameObject);
        }
        if (col.gameObject.name == "Buggy")
        {
            Debug.Log($"{gameObject.name} reached buggy at {DateTime.Now}");
        }
        
    }
}
