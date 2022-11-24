using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyScript : MonoBehaviour
{
    [SerializeField] private GameObject actionManager;
    [SerializeField] private ActionManager aMScript;
    [SerializeField] private Int32 unixTimestamp;

    void Awake()
    {
        aMScript = actionManager.GetComponent<ActionManager>();
    }
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }
    
    private void Jump()
    {
        unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        aMScript.Send_Jump(unixTimestamp);
    }
}
