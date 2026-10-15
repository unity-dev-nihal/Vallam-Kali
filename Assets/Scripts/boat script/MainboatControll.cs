using UnityEngine;

public class MainBoatController : MonoBehaviour
{
    [Header("Movement")]
    public float normalSpeed = 8f;
    public float goodSpeed = 9f;
    public float perfectSpeed = 11f;

    [Header("Turning")]
    public float turnSpeed = 50f;

    [Header("Boost")]
    public float boostSpeed = 15f;
    public float boostDuration = 2f;

    [Header("Beat Settings")]
    public float missSpeed = 5f;

    [Header("Finish")]
    public GameObject finishLine;
    public GameObject resultPanel;

    private float currentSpeed;
    private bool isBoosting;
    private float boostTimer;
    private bool raceFinished;

    private void Start()
    {
        currentSpeed = normalSpeed;

        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (raceFinished)
            return;

        MoveBoat();
        TurnBoat();

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;

            if (boostTimer <= 0)
            {
                StopBoost();
            }
        }

        CheckBoostInput();
    }

    private void MoveBoat()
    {
        // Move in the opposite direction of the boat's forward direction
        transform.position -= transform.forward * currentSpeed * Time.deltaTime;
    }

    private void TurnBoat()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        transform.Rotate(
            0f,
            horizontalInput * turnSpeed * Time.deltaTime,
            0f
        );
    }

    private void CheckBoostInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartBoost();
        }
    }

    // =========================
    // RHYTHM FUNCTIONS
    // =========================

    public void PerfectBeat()
    {
        if (isBoosting)
            return;

        currentSpeed = perfectSpeed;
    }

    public void GoodBeat()
    {
        if (isBoosting)
            return;

        currentSpeed = goodSpeed;
    }

    public void MissBeat()
    {
        if (isBoosting)
            return;

        currentSpeed = missSpeed;
    }

    // =========================
    // BOOST
    // =========================

    private void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        currentSpeed = boostSpeed;
    }

    private void StopBoost()
    {
        isBoosting = false;
        currentSpeed = normalSpeed;
    }

    // =========================
    // FINISH LINE
    // =========================

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished)
            return;

        if (other.gameObject == finishLine)
        {
            FinishRace();
        }
    }

    private void FinishRace()
    {
        raceFinished = true;
        currentSpeed = 0f;

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        Debug.Log("Player finished the race!");
    }
}