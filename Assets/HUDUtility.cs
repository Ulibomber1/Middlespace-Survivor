using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDUtility : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumber, experienceFraction, moneyNumber, healthFraction;
    [SerializeField] private RectTransform experienceBar, healthBar;

    public delegate void HUDAwakeHandler(GameObject HUD);
    public static event HUDAwakeHandler OnHUDAwake;

    private void Awake()
    {
        OnHUDAwake?.Invoke(gameObject);
        PlayerController.OnPlayerDataChange += UpdateHUD;
    }

    private void UpdateHUD(float HP, float maxHP)
    {
        healthFraction.text = $"{HP}/{maxHP}";
        float healthBarScale = HP / maxHP;
        healthBar.localScale = new Vector3(healthBarScale, healthBar.localScale.y, healthBar.localScale.z);
    }

    private void Update()
    {
        
    }
}
