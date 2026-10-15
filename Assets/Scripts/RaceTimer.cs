using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    [Header("Timer UI")]
    public TMP_Text timerText;

    private float currentTime;
    private bool timerRunning;

    private void Start()
    {
        currentTime = 0f;
        timerRunning = true;

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        currentTime += Time.deltaTime;

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(currentTime / 3600f);
        int minutes = Mathf.FloorToInt((currentTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = string.Format(
            "{0:00}:{1:00}:{2:00}",
            hours,
            minutes,
            seconds
        );
    }

    public void StopTimer()
    {
        timerRunning = false;
        UpdateTimerDisplay();
    }

    public float GetFinalTime()
    {
        return currentTime;
    }
}