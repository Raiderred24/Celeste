using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public Collision_Check coll;
    public Rigidbody2D rb;
    private float moveInput;
    float wallPush = 0;
    float staminaTimer = 0;
    bool pushingWall = false;
    bool canMove = true;
    bool canDash = true;

    public float speed = 12;
    public float wallJumpDelay = 1;
    public float dashDelay = 1;
    public float jumpForce = 50;
    public float gravityDelay = 0.5f;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public float stamina = 200;
    public float staminaDelay = 100;
    public float dragAmount = 50;

    private bool groundTouch;
    private bool hasDashed;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision_Check>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        if (coll.onGround)
        {
            StopCoroutine(DelayDash(dashDelay / 4));
        }

        Walk(dir);

        if (canDash && Input.GetKeyDown(KeyCode.P))
        {
            Dash(xRaw, yRaw);
        }

        WallSlide();
    }
    private void Walk(Vector2 dir)
    {
        Vector2 movement;
        if (!canDash)
        {
            movement = new Vector2(rb.velocity.x, rb.velocity.y);
            Drag(dragAmount / 2);
        }
        else if (canMove)
            movement = new Vector2(dir.x * speed, rb.velocity.y);
        else
        {
            movement = new Vector2(rb.velocity.x, rb.velocity.y);
            Drag(dragAmount);
        }
        rb.velocity = movement;
    }

    IEnumerator DelayWallJump(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    IEnumerator DelayDash(float seconds)
    {
        canDash = false;
        yield return new WaitForSeconds(seconds);
        canDash = true;
    }

    IEnumerator DelayGravity(float seconds)
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(seconds);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void WallJump()
    {
        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        if (pushingWall)
        {
            rb.velocity = (wallDir * 3f + Vector2.up * 6f);
            StartCoroutine(DelayWallJump(wallJumpDelay));
        }
    }
    
    private void WallSlide()
    {
        pushingWall = false;
        if ((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
            wallPush++;
        }
        else if (staminaTimer > staminaDelay)
        {
            staminaTimer++;
        }
        else
        {
            staminaTimer = 0;
            wallPush = 0;
        }

        //float push = pushingWall ? 0 : rb.velocity.x;

        if (wallPush > stamina)
            rb.velocity = new Vector2(0, -slideSpeed);
    }

    private void Dash(float x, float y)
    {
        if (y < 0.1 && y > -0.1)
        {
            rb.velocity = new Vector2(rb.velocity.x + x * dashSpeed / 2, rb.velocity.y + y * dashSpeed * 2);
            StartCoroutine(DelayGravity(gravityDelay));
        }
        else if (x < 0.1)
        {
            if (y > 0)
                rb.velocity = new Vector2(rb.velocity.x + x * dashSpeed / 4, rb.velocity.y + y * dashSpeed);
            else
                rb.velocity = new Vector2(rb.velocity.x + x * dashSpeed / 4, rb.velocity.y / 2 + y * dashSpeed);
        }
        else
        {
            if (y > 0)
                rb.velocity = new Vector2(rb.velocity.x + x * dashSpeed / 2, rb.velocity.y + y * dashSpeed);
            else
                rb.velocity = new Vector2(rb.velocity.x + x * dashSpeed / 2, rb.velocity.y / 2 + y * dashSpeed);
        }
        StartCoroutine(DelayDash(dashDelay));
    }

    private void Drag(float amount)
    {
        float slow = rb.velocity.x / amount;
        rb.velocity = new Vector2(rb.velocity.x - slow, rb.velocity.y);
    }
}
