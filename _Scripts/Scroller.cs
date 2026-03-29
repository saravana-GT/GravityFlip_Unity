using UnityEngine;

public class Scroller : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.Playing) return;

        // Move the object to the left based on game speed
        transform.Translate(Vector3.left * GameManager.Instance.currentSpeed * Time.deltaTime);

        // If the object goes too far left, destroy it (unless it's the background which should loop)
        if (transform.position.x < -15f)
        {
            if (gameObject.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
        }
    }
}
