using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUtility : MonoBehaviour
{
    float replayCutsceneTimer;
    [SerializeField] private Button cutscene; 
    // Start is called before the first frame update
    void Awake()
    {
        replayCutsceneTimer = 0;
        cutscene.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        replayCutsceneTimer += Time.deltaTime;

        if (Input.anyKeyDown)
            replayCutsceneTimer = 0;

        if (replayCutsceneTimer > 120)
            ReloadCutscene();
    }

    void TaskOnClick()
    {
        ReloadCutscene();
    }

    private void ReloadCutscene()
    {
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("OpeningCutscene");
    }
}
