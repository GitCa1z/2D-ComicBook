using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class PlayerCollision : MonoBehaviour
{
    public int score = 0; // Player's score
    public Text scoreText; // UI Text to display the score
    public AudioSource blip;

    private void Start()
    {
        UpdateScoreText(); // Initialize the score display
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "PointObject"
        if (collision.gameObject.CompareTag("PointObject"))
        {
            score++; // Increment the score
            UpdateScoreText(); // Update the score on the UI
            blip.Play();

            // Optionally, destroy the object after collision
            Destroy(collision.gameObject);
        }
    }

    private void UpdateScoreText()
    {
        // Update the score text in the UI
        scoreText.text = score.ToString();
    }
}

