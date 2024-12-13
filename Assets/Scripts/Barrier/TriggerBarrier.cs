using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBarrier : MonoBehaviour
{
    public GameObject rectangle; // Assign the rectangle GameObject in the Inspector
    private Barrier barrier;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        if (rectangle == null)
        {
            Debug.LogError("Rectangle GameObject is not assigned!");
            enabled = false;
            return;
        }

        barrier = rectangle.GetComponent<Barrier>();
        spriteRenderer = rectangle.GetComponent<SpriteRenderer>();
        

        if (barrier == null)
        {
            Debug.LogError("RectangleMover script is not found on the assigned rectangle!");
            enabled = false;
            return;
        }

        // Disable the RectangleMover script at the start
        barrier.enabled = false;
        spriteRenderer.enabled = false;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Enable the RectangleMover script to start the movement
            barrier.enabled = true;
            spriteRenderer.enabled=true;

            // Optional: Destroy the trigger after activating the rectangle
            Destroy(gameObject);
        }
    }
}
