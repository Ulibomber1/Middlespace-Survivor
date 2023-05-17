using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUtility : MonoBehaviour
{
    float replayCutsceenTimer;
    [SerializeField] private Button cutscene; 
    // Start is called before the first frame update
    void Awake()
    {
        replayCutsceenTimer = 0;
        cutscene.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        replayCutsceenTimer += Time.deltaTime;

        if (Input.anyKeyDown)
            replayCutsceenTimer = 0;

        if (replayCutsceenTimer > 120)
            ReloadCutsceen();
    }

    void TaskOnClick()
    {
        ReloadCutsceen();
    }

    private void ReloadCutsceen()
    {
        SceneManager.LoadScene("OpeningCutscene");
    }
}
