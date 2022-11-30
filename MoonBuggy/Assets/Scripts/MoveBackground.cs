using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{

    [SerializeField] private float speed;
    private Transform _backTransform;
    private float _backSize;
    private float _backPos;

    void Start()
    {
        _backTransform = GetComponent<Transform>();
        _backSize = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Move()
    {
        _backPos -= speed * Time.deltaTime;
        _backPos = Mathf.Repeat(_backPos, _backSize);
        _backTransform.position = new Vector3(_backPos,-2.0432f , 0);
    }

    void Update()
    {
        Move();
    }

    

}
