using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform mapParent; 

    public float spawnInterval = 2f;
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    private float currentObstacleSpeed;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnInterval);
        SetRandomObstacleSpeed();
    }

    public void SetRandomObstacleSpeed()
    {
        currentObstacleSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void SpawnObstacle()
    {
        Vector3 spawnPosition = new Vector3(30f, 0.5f, 0f);

        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, mapParent);

        Rigidbody rb = obstacle.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            rb.linearVelocity = Vector3.left * currentObstacleSpeed;
        }
        else
        {
            Debug.LogError("Obstacle prefab has no Rigidbody!");
        }
    }
}
