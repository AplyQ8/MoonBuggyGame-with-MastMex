using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyScript : MonoBehaviour
{
    [SerializeField] private GameObject actionManager;
    [SerializeField] private ActionManager aMScript;
    [SerializeField] private Int32 unixTimestamp;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight = 10f;

    void Awake()
    {
        aMScript = actionManager.GetComponent<ActionManager>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody2D.gravityScale));
    }
    public void Update()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody2D.gravityScale));
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //Debug.Log($"Ended jump at: {DateTime.Now.Second}:{DateTime.Now.Millisecond}");
        }

        
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            //Debug.Log($"Started jump at: {DateTime.Now.Second}:{DateTime.Now.Millisecond}");
        }
    }

    private void Jump()
    {
        unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        aMScript.Send_Jump(unixTimestamp);
        rigidBody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
