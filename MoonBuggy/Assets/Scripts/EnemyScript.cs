using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject statusImage;
    [SerializeField] private Sprite checkMark;
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


}
