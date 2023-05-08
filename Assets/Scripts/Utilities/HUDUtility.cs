using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDUtility : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumber, experienceFraction, moneyNumber, healthFraction, timer;
    [SerializeField] private RectTransform experienceBar, healthBar;

    public delegate void HUDAwakeHandler(GameObject HUD);
    public static event HUDAwakeHandler OnHUDAwake;

    private void Awake()
    {
        OnHUDAwake?.Invoke(gameObject);
        PlayerController.OnPlayerDataChange += UpdatePlayerInfo;
        PlayerController.OnLevelUp += UpdateLevel;
        GameManager.Instance.OnTimerUpdate += UpdateTimer;
        GameManager.Instance.OnCreditsUpdated += UpdateCredits;
        moneyNumber.text = $"$: {GameManager.Instance.GetCurrentCredits()}";
    }

    private void UpdatePlayerInfo(float HP, float maxHP, double experience, double maxExperience)
    {

        healthFraction.text = $"{(int)HP}/{(int)maxHP}";
        float healthBarScale = HP / maxHP;
        healthBar.gameObject.GetComponent<Slider>().value = healthBarScale;

        experienceFraction.text = $"{(int)experience}/{(int)maxExperience}";
        float experienceBarScale = (float)(experience / maxExperience);
        experienceBar.gameObject.GetComponent<Slider>().value = experienceBarScale;
    }

    private void UpdateCredits(int credits)
    {
        Debug.Log("UpdateCredits Reached!");
        moneyNumber.text = $"$: {credits}";
    }

    private void UpdateLevel(int newLevel)
    {
        levelNumber.text = $"Lvl: {newLevel.ToString()}";
    }

    private void UpdateTimer(float remainingTime)
    {
        int totalSeconds = (int)remainingTime;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();

        if (minutes <= 0)
            minutesString = "00";
        else if (minutesString.Length == 1)
            minutesString = "0" + minutesString;

        if (seconds <= 0)
            secondsString = "00";
        else if (secondsString.Length == 1)
            secondsString = "0" + secondsString;

        timer.text = $"{minutesString}:{secondsString}";
    }

    private void OnDestroy()
    {
        // This fixes the HP bar bug, but I don't know why. Needs further investigation/research.
        // Update: It prevents the delegate from accessing a previous instance of HUDUtility
        PlayerController.OnPlayerDataChange -= UpdatePlayerInfo;
        GameManager.Instance.OnTimerUpdate -= UpdateTimer;
        PlayerController.OnLevelUp -= UpdateLevel;
        GameManager.Instance.OnCreditsUpdated -= UpdateCredits;
    }
    private void Update()
    {
        
    }
}
