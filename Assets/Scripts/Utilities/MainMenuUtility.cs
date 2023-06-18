using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Global Enums

public class MainMenuUtility : MonoBehaviour
{
    // Private Enums

    // References
    [SerializeField] private Button cutscene;

    // Internal Variables
    float replayCutsceenTimer;

    // User-defined Objects

    // Delegates

    // Events

    // Unity Methods
    void Awake()
    {
        replayCutsceenTimer = 0;
        cutscene.onClick.AddListener(TaskOnClick);
    }
    void Update()
    {
        replayCutsceenTimer += Time.deltaTime;

        if (Input.anyKeyDown)
            replayCutsceenTimer = 0;

        if (replayCutsceenTimer > 120)
            ReloadCutsceen();
    }

    // User-defined Methods
    void TaskOnClick()
    {
        ReloadCutsceen();
    }

    private void ReloadCutsceen()
    {
        SceneManager.LoadScene("OpeningCutscene");
    }
}
