using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Check : MonoBehaviour
{

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public float collisionRadius = 0.25f;
    public LayerMask groundLayer;
    public int wallSide;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
    }
}
