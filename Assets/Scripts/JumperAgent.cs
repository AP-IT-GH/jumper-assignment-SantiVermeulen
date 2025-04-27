using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class JumperAgent : Agent
{
    private Rigidbody rb;
    public float jumpForce = 8f;

    private bool isGrounded;

    public ObstacleSpawner spawner; // Assign your spawner here

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        if (spawner != null)
        {
            spawner.SetRandomObstacleSpeed();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // No need to add manual observations if you are using RaySensor!
        // If you still want to observe nearest obstacle distance + velocity, you can uncomment:

        // GameObject nearestObstacle = FindClosestObstacle();
        // if (nearestObstacle != null)
        // {
        //     float distance = nearestObstacle.transform.position.x - transform.position.x;
        //     sensor.AddObservation(distance);
        // }
        // else
        // {
        //     sensor.AddObservation(10f);
        // }

        // sensor.AddObservation(rb.linearVelocity.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int jumpAction = actions.DiscreteActions[0];

        if (jumpAction == 1 && isGrounded)
        {
            Jump();
        }

        AddReward(0.001f); // Small reward for surviving
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Agent jumped!");
    }

    // NEW collision-based ground check:

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private GameObject FindClosestObstacle()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obstacle in obstacles)
        {
            float distance = obstacle.transform.position.x - transform.position.x;
            if (distance > 0 && distance < minDistance)
            {
                minDistance = distance;
                closest = obstacle;
            }
        }

        return closest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}
