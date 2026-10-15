using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class RaceResultsUI : MonoBehaviour
{
    [Header("Results UI")]
    [SerializeField] private GameObject resultsOverlay;
    [SerializeField] private TMP_Text placeText;
    [SerializeField] private TMP_Text statsText;

    [Header("Temporary Test Values")]
    [SerializeField] private bool enableTestKey = true;
    [SerializeField] private int testPlace = 1;
    [SerializeField] private float testRaceTime = 134.56f;
    [SerializeField] private int testPerfectRows = 42;
    [SerializeField] private int testBoostsUsed = 3;

    private bool raceFinished;

    private void Start()
    {
        resultsOverlay.SetActive(false);
    }

    private void Update()
    {
        // Temporary: press R to preview the Result panel.
        if (enableTestKey && !raceFinished &&
            Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            ShowResults(testPlace, testRaceTime, testPerfectRows, testBoostsUsed);
        }
    }

    public void ShowResults(int place, float raceTime, int perfectRows, int boostsUsed)
    {
        if (raceFinished) return;

        raceFinished = true;

        placeText.text = GetPlaceText(place);

        int minutes = Mathf.FloorToInt(raceTime / 60f);
        float seconds = raceTime % 60f;

        statsText.text =
            "RACE TIME       " + minutes.ToString("00") + ":" + seconds.ToString("00.00") + "\n" +
            "PERFECT ROWS    " + perfectRows + "\n" +
            "BOOSTS USED     " + boostsUsed;

        resultsOverlay.SetActive(true);
        Time.timeScale = 0f;
    }

    private string GetPlaceText(int place)
    {
        switch (place)
        {
            case 1: return "1ST PLACE";
            case 2: return "2ND PLACE";
            case 3: return "3RD PLACE";
            default: return place + "TH PLACE";
        }
    }

    public void RetryRace()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}