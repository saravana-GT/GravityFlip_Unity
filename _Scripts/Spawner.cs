using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.5f;
    public float minY = -3f;
    public float maxY = 3f;

    private float timer;

    void Update()
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.Playing) return;

        timer += Time.deltaTime;

        // Reduce spawn interval slightly as speed increases
        float adjustedInterval = spawnInterval / (GameManager.Instance.currentSpeed / 10f);

        if (timer >= adjustedInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        // Randomly choose Floor or Ceiling for the spike in 3D
        bool top = Random.value > 0.5f;
        float yPos = top ? maxY : minY;
        
        Vector3 spawnPos = new Vector3(transform.position.x, yPos, transform.position.z);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // If it's on the ceiling, rotate the 3D obstacle upside down
        if (top)
        {
            obstacle.transform.Rotate(180f, 0, 0);
        }
    }
}
