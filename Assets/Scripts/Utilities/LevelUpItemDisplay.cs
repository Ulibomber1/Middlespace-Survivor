using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

//Global Enums

public class LevelUpItemDisplay : MonoBehaviour
{
    // Private Enums

    // References
    AudioSource src;
    ItemDataUtility itemData;
    [SerializeField] TextMeshProUGUI Item;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] GameObject LevelUpUI;

    // Internal Variables
    bool isDisplaying;
    string currentItemName;
    string currentDescription;
    [SerializeField] float textSpeed;

    // User-defined Objects

    // Delegates

    // Events

    // Unity Methods
    void Awake()
    {
        //Item = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        //Description = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        src = GetComponent<AudioSource>();
        MouseOver.OnItemMouseover += DisplayDescription;
        ItemDataUtility.NotEnoughCredits += NotEnough;
        Item.text = string.Empty;
        Description.text = string.Empty;
        isDisplaying = false;
        if (LevelUpUI.TryGetComponent(out ItemDataUtility ItemDat))
            itemData = ItemDat;
    }
    private void OnDestroy()
    {
        MouseOver.OnItemMouseover -= DisplayDescription;
        ItemDataUtility.NotEnoughCredits -= NotEnough;
    }

    // User-defined Methods
    IEnumerator TypeLine()
    {
        Item.text += currentItemName;

        foreach (char c in currentDescription.ToCharArray())
        {
            Description.text += c;
            src.Play();
            yield return new WaitForSeconds(textSpeed);
        }

        isDisplaying = false;
    }
    void DisplayDescription(string itemName, bool unused)
    {
        currentItemName = itemName;
        currentDescription = itemData.GetItemBlurb(itemName);
        StartDescription();
    }
    void NotEnough(string notEnough, int creditsNeeded)
    {
        currentItemName = notEnough;
        currentDescription =
            "You do not have enough credits to buy this equpment.\nCredits needed: " + creditsNeeded + ".";
        StartDescription();
    }
    void StartDescription()
    {
        if (isDisplaying)
            StopAllCoroutines();

        Item.text = string.Empty;
        Description.text = string.Empty;
        isDisplaying = true;

        StartCoroutine(TypeLine());
    }
    /*private void OnEnable()
    {
        dialogueObject = GameObject.Find("/Dialogue");
        currentDialogue = dialogueObject.GetComponent<DialoguePrompt>().GetDialogue();
        src = GetComponent<AudioSource>();

        FileInfo dialogueFile = new FileInfo("Assets\\Dialogue\\" + currentDialogue + ".txt");
        StreamReader reader = dialogueFile.OpenText();

        lines = new List<string>();

        for (string text = reader.ReadLine(); text != null; text = reader.ReadLine())
            lines.Add(text);

        StartDialogue();
    }*/

    // Update is called once per frame
    /*void Update()
    {
        textTimeout += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || textTimeout > 2.5)
        {
            if (Speaking.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                Speaking.text = lines[index];
            }
            textTimeout = 0;
        }
    }*/

    /*void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            Speaker.text = string.Empty;
            Speaking.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Speaker.text = string.Empty;
            Speaking.text = string.Empty;
            lines.Clear();
            dialogueObject.GetComponent<DialoguePrompt>().SetDialogue("");
            gameObject.SetActive(false);
        }
    }*/
}
