using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Barrier : MonoBehaviour
{
    public PolygonCollider2D polygonCollider; // Assign the PolygonCollider2D in the Inspector
    public float speed = 2f; // Movement speed of the rectangle

    public enum MovementDirection { LeftToRight, RightToLeft, TopToBottom, BottomToTop }
    public MovementDirection movementDirection = MovementDirection.LeftToRight; // Set the movement direction

    private SpriteRenderer spriteRenderer;
    private float originalHeight, originalWidth; // The original dimensions of the rectangle sprite

    private float colliderLeftX, colliderRightX, colliderTopY, colliderBottomY;

    void Start()
    {
        if (polygonCollider == null)
        {
            Debug.LogError("PolygonCollider2D is not assigned!");
            enabled = false;
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache the original dimensions of the rectangle sprite
        originalHeight = spriteRenderer.bounds.size.y / transform.localScale.y;
        originalWidth = spriteRenderer.bounds.size.x / transform.localScale.x;

        // Calculate the bounds of the collider
        Vector2[] points = polygonCollider.points;
        colliderLeftX = float.MaxValue;
        colliderRightX = float.MinValue;
        colliderTopY = float.MinValue;
        colliderBottomY = float.MaxValue;

        foreach (var point in points)
        {
            Vector2 worldPoint = polygonCollider.transform.TransformPoint(point);
            colliderLeftX = Mathf.Min(colliderLeftX, worldPoint.x);
            colliderRightX = Mathf.Max(colliderRightX, worldPoint.x);
            colliderBottomY = Mathf.Min(colliderBottomY, worldPoint.y);
            colliderTopY = Mathf.Max(colliderTopY, worldPoint.y);
        }

        // Set the starting position and stretch the rectangle
        SetInitialPositionAndStretch();
    }

    void Update()
    {
        Vector3 position = transform.position;

        // Move the rectangle based on the selected direction
        switch (movementDirection)
        {
            case MovementDirection.LeftToRight:
                position.x += speed * Time.deltaTime;
                if (position.x > colliderRightX) Destroy(gameObject);
                break;
            case MovementDirection.RightToLeft:
                position.x -= speed * Time.deltaTime;
                if (position.x < colliderLeftX) Destroy(gameObject);
                break;
            case MovementDirection.TopToBottom:
                position.y -= speed * Time.deltaTime;
                if (position.y < colliderBottomY) Destroy(gameObject);
                break;
            case MovementDirection.BottomToTop:
                position.y += speed * Time.deltaTime;
                if (position.y > colliderTopY) Destroy(gameObject);
                break;
        }

        transform.position = position;
    }

    void SetInitialPositionAndStretch()
    {
        Vector3 scale = transform.localScale;
        Vector3 position = transform.position;

        switch (movementDirection)
        {
            case MovementDirection.LeftToRight:
            case MovementDirection.RightToLeft:
                // Stretch vertically
                float height = colliderTopY - colliderBottomY;
                scale.y = height / originalHeight;
                position.y = colliderBottomY + height / 2; // Center vertically
                break;

            case MovementDirection.TopToBottom:
            case MovementDirection.BottomToTop:
                // Stretch horizontally
                float width = colliderRightX - colliderLeftX;
                scale.x = width / originalWidth;
                position.x = colliderLeftX + width / 2; // Center horizontally
                break;
        }

        // Set the initial position based on the direction
        switch (movementDirection)
        {
            case MovementDirection.LeftToRight:
                position.x = colliderLeftX; // Start at the left
                break;
            case MovementDirection.RightToLeft:
                position.x = colliderRightX; // Start at the right
                break;
            case MovementDirection.TopToBottom:
                position.y = colliderTopY; // Start at the top
                break;
            case MovementDirection.BottomToTop:
                position.y = colliderBottomY; // Start at the bottom
                break;
        }

        // Apply the new scale and position
        transform.localScale = scale;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
