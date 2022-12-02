using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuggyScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private float jumpHeight = 25f;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBody2D.gravityScale));
    }
    public void Jump()
    {
        rigidBody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
