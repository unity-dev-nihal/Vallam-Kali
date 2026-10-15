using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceCountdownUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image countdownImage;

    [Header("Countdown Sprites")]
    [SerializeField] private Sprite threeSprite;
    [SerializeField] private Sprite twoSprite;
    [SerializeField] private Sprite oneSprite;
    [SerializeField] private Sprite goSprite;

    private IEnumerator Start()
    {
        countdownImage.enabled = true;

        countdownImage.sprite = threeSprite;
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = twoSprite;
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = oneSprite;
        yield return new WaitForSeconds(1f);

        countdownImage.sprite = goSprite;
        yield return new WaitForSeconds(0.8f);

        countdownImage.enabled = false;
    }
}