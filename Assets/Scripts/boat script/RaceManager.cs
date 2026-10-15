using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [Header("Race Boats")]
    public Transform playerBoat;
    public Transform[] allBoats;

    [Header("Race Waypoints")]
    public Transform[] waypoints;

    [Header("UI")]
    public TMP_Text positionText;
    public TMP_Text resultPositionText;

    [Header("Result Panel")]
    public GameObject resultPanel;

    private bool playerFinished;
    private int finalPosition;

    private void Start()
    {
        resultPanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerFinished)
        {
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        int playerPosition = GetBoatPosition(playerBoat);

        positionText.text = playerPosition + " / " + allBoats.Length;
    }

    public void FinishPlayer()
    {
        if (playerFinished)
            return;

        playerFinished = true;

        finalPosition = GetBoatPosition(playerBoat);

        resultPositionText.text = finalPosition + " / " + allBoats.Length;

        resultPanel.SetActive(true);
    }

    private int GetBoatPosition(Transform boat)
    {
        float playerProgress = GetProgress(boat);

        int position = 1;

        foreach (Transform otherBoat in allBoats)
        {
            if (otherBoat == boat)
                continue;

            float otherProgress = GetProgress(otherBoat);

            if (otherProgress > playerProgress)
            {
                position++;
            }
        }

        return position;
    }

    private float GetProgress(Transform boat)
    {
        int closestWaypoint = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(
                boat.position,
                waypoints[i].position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = i;
            }
        }

        return closestWaypoint;
    }
}