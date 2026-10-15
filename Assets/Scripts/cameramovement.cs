using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [Header("Player Boat")]
    public Transform player;

    [Header("Camera Position")]
    public float distanceBehind = 10f;
    public float height = 3f;
    public float followSmoothness = 5f;

    [Header("Camera Look")]
    public float lookAhead = 5f;

    [Header("Turning")]
    public float turnMoveAmount = 1f;
    public float turnTiltAmount = 5f;
    public float turnSmoothness = 5f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        FollowBoat();
    }

    void FollowBoat()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // The boat moves in the opposite direction of player.forward
        Vector3 movementDirection = -player.forward;

        // Camera stays BEHIND the boat's actual movement direction
        Vector3 targetPosition =
            player.position
            - movementDirection * distanceBehind
            + Vector3.up * height
            + player.right * horizontalInput * turnMoveAmount;

        // Smooth camera movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSmoothness * Time.deltaTime
        );

        // Camera looks toward the FRONT of the boat's movement direction
        Vector3 lookPosition =
            player.position
            + movementDirection * lookAhead;

        Vector3 direction =
            lookPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            // Small camera tilt while turning
            targetRotation *= Quaternion.Euler(
                0f,
                0f,
                -horizontalInput * turnTiltAmount
            );

            // Smooth camera rotation
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                turnSmoothness * Time.deltaTime
            );
        }
    }
}