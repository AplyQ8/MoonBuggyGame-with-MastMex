using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatationClass : MonoBehaviour
{
    public void Awake()
    {
        var rect = GetComponent<SpriteRenderer>().sprite.rect.size;
        
        Debug.Log($"{gameObject.name}: width: {rect.x} height: {rect.y}");
    }
}
