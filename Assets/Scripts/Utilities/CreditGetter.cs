using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditGetter : MonoBehaviour
{
    //Public

    //Private
    TextMeshProUGUI textMesh;

    private void Awake()
    {
        GameManager.Instance.OnCreditsUpdated += UpdateText;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = $"$: {GameManager.Instance.GetCurrentCredits()}";
    }
    
    private void UpdateText(int credits)
    {
        textMesh.text = $"$: {credits}";
    }
}
