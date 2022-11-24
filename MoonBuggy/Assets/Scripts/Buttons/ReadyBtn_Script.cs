using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn_Script : MonoBehaviour
{
    
    [SerializeField] private GameObject actionManager;
    
    public void Ready()
    {
        actionManager.GetComponent<ActionManager>().Send_Ready();
    }
}
