using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] public  Rigidbody2D rb2D;
  [SerializeField] public  Transform groundCheck;
  [SerializeField] public  LayerMask groundMask;
  [SerializeField] public  TrailRenderer trilRenderer;
  [SerializeField] public Transform wallCheck;
  [SerializeField] public LayerMask wallMask;

    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 8f;
    private bool isFacingRight = true;
    [SerializeField ]private int extraJumps = 1;



    private bool canDash = true;
    private bool isDashing;
    private bool isWallSliding;
    private bool isWallJumping;

    private float wallSlidingSpeed = 2f;
    private float dashPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCoolDown = 1f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(16f, 8f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JumpReset();
        wallJump();

        if (!isWallJumping)
        {
            Flip();

        }


    }

    private void JumpReset()
    {
        if (isGrounded()) extraJumps = 1;
    }

    private void FixedUpdate()
    {
        if (isDashing )
        {
            return;
        }

        if (!isWallJumping)
        {
         rb2D.velocity = new Vector2(horizontal * speed, rb2D.velocity.y);
        }

        

        wallSliding();

       
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (context.performed && extraJumps > 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
            extraJumps--;
           
        }

        if (wallJumpingCounter > 0)
        {
            isWallJumping = true;
            rb2D.velocity = new Vector2 (wallJumpingDirection * wallJumpingPower.x,wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDuration)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;

            }

            Invoke(nameof(stopWallJumping), wallJumpingDirection);

        }

    }

    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundMask);
    }

    public bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallMask);
    }

    public void wallSliding()
    {
        if (isWalled() && !isGrounded() && horizontal != 0) { 
            isWallSliding = true;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        else isWallSliding = false;
    }

    private void wallJump()
    {
        if (isWallSliding) { 
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(stopWallJumping));
        }

        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    }

    private void stopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if ( isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {

            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

      
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Dashing(InputAction.CallbackContext context)
    {
        
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb2D.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        trilRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trilRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }

    
}
