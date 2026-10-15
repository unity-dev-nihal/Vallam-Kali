using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class RhythmRowingController : MonoBehaviour
{
    [Header("Beat Timing")]
    [SerializeField] private float bpm = 90f;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float perfectWindow = 0.12f;
    [SerializeField] private float goodWindow = 0.24f;

    [Header("Rhythm Bar")]
    [SerializeField] private Image[] rhythmBeats;
    [SerializeField] private Sprite activeBeatSprite;
    [SerializeField] private TMP_Text comboText;

    [Header("Feedback Images")]
    [SerializeField] private Image feedbackImage;
    [SerializeField] private Sprite perfectSprite;
    [SerializeField] private Sprite goodSprite;
    [SerializeField] private Sprite missSprite;
    [SerializeField] private float feedbackDuration = 0.55f;

    [Header("Boost")]
    [SerializeField] private BoostUIController boostUI;
    [SerializeField] private float perfectBoostGain = 20f;
    [SerializeField] private float goodBoostGain = 10f;

    [Header("Race Music")]
    [SerializeField] private AudioSource raceMusic;

    private Sprite[] idleBeatSprites;
    private float beatInterval;
    private float rhythmStartTime;

    private int currentVisualBeat = -1;
    private int nextBeatToResolve;
    private int combo;

    private Coroutine feedbackRoutine;

    private void Start()
    {
        if (rhythmBeats == null || rhythmBeats.Length == 0)
        {
            Debug.LogWarning("No rhythm beats assigned.");
            enabled = false;
            return;
        }

        beatInterval = 60f / bpm;
        rhythmStartTime = Time.time + startDelay;

        idleBeatSprites = new Sprite[rhythmBeats.Length];

        for (int i = 0; i < rhythmBeats.Length; i++)
        {
            idleBeatSprites[i] = rhythmBeats[i].sprite;
        }

        if (comboText != null)
            comboText.text = "COMBO x0";

        if (feedbackImage != null)
            feedbackImage.gameObject.SetActive(false);

        if (raceMusic != null)
        {
            raceMusic.loop = true;
            raceMusic.playOnAwake = false;
            StartCoroutine(StartRaceMusic());
        }
    }

    private void Update()
    {
        if (Time.time < rhythmStartTime)
            return;

        UpdateBeatVisual();
        CheckMissedBeat();

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckRowInput();
        }
    }

    private IEnumerator StartRaceMusic()
    {
        yield return new WaitForSeconds(startDelay);

        if (raceMusic != null)
            raceMusic.Play();
    }

    private void UpdateBeatVisual()
    {
        float timeSinceStart = Time.time - rhythmStartTime;
        int beatNumber = Mathf.FloorToInt(timeSinceStart / beatInterval);

        if (beatNumber == currentVisualBeat)
            return;

        currentVisualBeat = beatNumber;

        int activeIndex = beatNumber % rhythmBeats.Length;

        for (int i = 0; i < rhythmBeats.Length; i++)
        {
            rhythmBeats[i].sprite = idleBeatSprites[i];
            rhythmBeats[i].transform.localScale = Vector3.one;
        }

        rhythmBeats[activeIndex].sprite = activeBeatSprite;
        rhythmBeats[activeIndex].transform.localScale = Vector3.one * 1.25f;

        StartCoroutine(ResetBeatVisual(activeIndex));
    }

    private void CheckRowInput()
    {
        float timeSinceStart = Time.time - rhythmStartTime;
        int nearestBeat = Mathf.RoundToInt(timeSinceStart / beatInterval);

        if (nearestBeat < nextBeatToResolve)
            return;

        float beatTime = rhythmStartTime + nearestBeat * beatInterval;
        float timingError = Mathf.Abs(Time.time - beatTime);

        nextBeatToResolve = nearestBeat + 1;

        if (timingError <= perfectWindow)
        {
            PerfectRow();
        }
        else if (timingError <= goodWindow)
        {
            GoodRow();
        }
        else
        {
            MissRow();
        }
    }

    private void CheckMissedBeat()
    {
        float expectedBeatTime =
            rhythmStartTime + nextBeatToResolve * beatInterval;

        if (Time.time > expectedBeatTime + goodWindow)
        {
            nextBeatToResolve++;
            MissRow();
        }
    }

    private void PerfectRow()
    {
        combo++;

        if (comboText != null)
            comboText.text = "COMBO x" + combo;

        if (boostUI != null)
            boostUI.AddBoost(perfectBoostGain);

        ShowFeedback(perfectSprite);
    }

    private void GoodRow()
    {
        combo++;

        if (comboText != null)
            comboText.text = "COMBO x" + combo;

        if (boostUI != null)
            boostUI.AddBoost(goodBoostGain);

        ShowFeedback(goodSprite);
    }

    private void MissRow()
    {
        combo = 0;

        if (comboText != null)
            comboText.text = "COMBO x0";

        ShowFeedback(missSprite);
    }

    private void ShowFeedback(Sprite sprite)
    {
        if (feedbackImage == null || sprite == null)
            return;

        feedbackImage.sprite = sprite;
        feedbackImage.gameObject.SetActive(true);

        if (feedbackRoutine != null)
            StopCoroutine(feedbackRoutine);

        feedbackRoutine = StartCoroutine(HideFeedback());
    }

    private IEnumerator ResetBeatVisual(int beatIndex)
    {
        yield return new WaitForSeconds(0.15f);

        if (beatIndex < rhythmBeats.Length)
        {
            rhythmBeats[beatIndex].sprite = idleBeatSprites[beatIndex];
            rhythmBeats[beatIndex].transform.localScale = Vector3.one;
        }
    }

    private IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(feedbackDuration);

        if (feedbackImage != null)
            feedbackImage.gameObject.SetActive(false);
    }
}