using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isUpsideDown = false;
    public float gravityScale = 7f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Using mass-specific gravity simulation for 3D
        rb.useGravity = false; 
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.Playing) return;

        // Apply custom gravity based on flip state
        Vector3 gravityForce = isUpsideDown ? Vector3.up : Vector3.down;
        rb.AddForce(gravityForce * gravityScale * Physics.gravity.magnitude, ForceMode.Acceleration);
    }

    void Update()
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.Playing) return;

        // One-touch control (Screen tap, Mouse click, or Spacebar)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            FlipGravity();
        }
    }

    void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;

        // Smoothly rotate the player character 180 degrees in 3D
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z += 180f;
        transform.eulerAngles = currentRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.EndGame();
        }
    }
}
