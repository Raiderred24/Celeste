using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Character_Movement movement = null;

    [Range(1, 20)]
    public float jumpVelocity;
    public int jumpLimit = 2;
    int jumpValue = 0;

    private void Start()
    {
        jumpLimit = jumpValue;
        GetComponent<Rigidbody2D>();
        movement = GetComponent<Character_Movement>();
    }
    void Update()
    {
        if(movement != null && movement.coll.onGround == true)
        {
            jumpValue = jumpLimit;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpValue <= jumpLimit)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
                jumpValue++;
            }
            if (movement != null)
            {
                movement.WallJump();
                jumpValue++;
            }
        }
    }
}
