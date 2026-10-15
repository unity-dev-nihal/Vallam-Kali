using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BoostUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider boostSlider;
    [SerializeField] private Image boostButtonImage;
    [SerializeField] private Button boostButton;

    [Header("Button Sprites")]
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite activeSprite;

    [Header("Boost Settings")]
    [SerializeField] private float maxBoost = 100f;
    [SerializeField] private float boostGainPerRow = 20f;
    [SerializeField] private float boostDuration = 2.5f;

    [Header("Temporary UI Testing")]
    [SerializeField] private bool enableTestControls = true;

    private float currentBoost;
    private bool isBoostActive;

    public bool IsBoostActive => isBoostActive;

    private void Start()
    {
        SetBoost(0f);
    }

    private void Update()
    {
        if (enableTestControls && Keyboard.current != null)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && !isBoostActive)
            {
                AddBoost(boostGainPerRow);
            }

            if (Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                TryUseBoost();
            }
        }

        if (isBoostActive)
        {
            float drainPerSecond = maxBoost / boostDuration;
            currentBoost -= drainPerSecond * Time.deltaTime;

            if (currentBoost <= 0f)
            {
                currentBoost = 0f;
                isBoostActive = false;
            }

            RefreshUI();
        }
    }

    public void AddBoost(float amount)
    {
        if (isBoostActive)
            return;

        currentBoost = Mathf.Clamp(currentBoost + amount, 0f, maxBoost);
        RefreshUI();
    }

    public void SetBoost(float amount)
    {
        currentBoost = Mathf.Clamp(amount, 0f, maxBoost);
        RefreshUI();
    }

    public bool TryUseBoost()
    {
        if (currentBoost < maxBoost || isBoostActive)
            return false;

        isBoostActive = true;
        RefreshUI();
        return true;
    }

    public void PressBoostButton()
    {
        TryUseBoost();
    }

    private void RefreshUI()
    {
        boostSlider.value = currentBoost;

        bool isReady = currentBoost >= maxBoost;

        if (isBoostActive)
        {
            boostButtonImage.sprite = activeSprite;
            boostButton.interactable = false;
        }
        else if (isReady)
        {
            boostButtonImage.sprite = readySprite;
            boostButton.interactable = true;
        }
        else if (currentBoost > 0f)
        {
            boostButtonImage.sprite = normalSprite;
            boostButton.interactable = false;
        }
        else
        {
            boostButtonImage.sprite = disabledSprite;
            boostButton.interactable = false;
        }
    }
}