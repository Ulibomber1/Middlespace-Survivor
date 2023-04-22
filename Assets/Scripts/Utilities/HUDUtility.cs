using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        GameManager.Instance.OnTimerUpdate += UpdateTimer;
    }

    private void UpdatePlayerInfo(float HP, float maxHP)
    {
        healthFraction.text = $"{HP}/{maxHP}";
        float healthBarScale = HP / maxHP;
        healthBar.localScale = new Vector3(healthBarScale, healthBar.localScale.y, healthBar.localScale.z);
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
        PlayerController.OnPlayerDataChange -= UpdatePlayerInfo;
        GameManager.Instance.OnTimerUpdate -= UpdateTimer;
    }
    private void Update()
    {
        
    }
}
