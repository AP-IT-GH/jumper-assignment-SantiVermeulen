using UnityEngine;

public class ObstacleCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
            Debug.Log("Obstacle destroyed by cleaner.");
        }
    }
}
