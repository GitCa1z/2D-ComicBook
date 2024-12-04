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

    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 16f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCoolDown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb2D.velocity = new Vector2(horizontal * speed, rb2D.velocity.y);

        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpPower);
        }

        if (context.canceled && rb2D.velocity.y > 0f) 
        { 
            rb2D.velocity = new Vector2(rb2D.velocity.x,rb2D.velocity.y * 0.5f);
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundMask);
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
