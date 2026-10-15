using UnityEngine;

public class BoatAI : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float speed = 11f;

    [Header("Turning")]
    public float rotationSpeed = 3f;

    [Header("Waypoint Settings")]
    public float waypointDistance = 2f;

    private int currentWaypoint = 0;

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        MoveBoat();
        RotateBoat();
        CheckWaypoint();
    }

    // =========================
    // MOVE BOAT
    // =========================

    private void MoveBoat()
    {
        // FIX:
        // Your boat model is facing opposite to its local forward.
        transform.position -= transform.forward * speed * Time.deltaTime;
    }

    // =========================
    // ROTATE BOAT
    // =========================

    private void RotateBoat()
    {
        Vector3 direction =
            waypoints[currentWaypoint].position - transform.position;

        // Keep boat level
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        // Because the boat moves using -transform.forward,
        // rotate the boat's forward direction opposite to the waypoint.
        Quaternion targetRotation =
            Quaternion.LookRotation(-direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    // =========================
    // CHECK WAYPOINT
    // =========================

    private void CheckWaypoint()
    {
        float distance = Vector3.Distance(
            transform.position,
            waypoints[currentWaypoint].position
        );

        if (distance <= waypointDistance)
        {
            currentWaypoint++;

            // If all waypoints are completed,
            // restart from the first waypoint.
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
}