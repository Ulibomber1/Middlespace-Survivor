using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUtility : MonoBehaviour
{
    private string currentSelection = null;
    private string currentMouseOver = null;

    public delegate void LevelUpGUIHandler(GameObject gui);
    public static event LevelUpGUIHandler OnAwake;
    private void Awake()
    {
        OnAwake?.Invoke(gameObject);
        MouseOver.OnItemMouseover += ChangeItemSelection;
        GameManager.OnDataReady += InputItemData;
    }

    private void InputItemData (List<string> names)
    {
        
    }

    private void ChangeItemSelection(string name, bool isAddable)
    {
        if (isAddable) 
        { 
            currentSelection = name;
            Debug.Log("Current Selection changed!");
        }
        currentMouseOver = name;
        Debug.Log("Mouseover target changed!");
    }

    private void ConfirmSelection(string selection)
    {
        // Add the selected item to character for the run
        Debug.Log("Item added!");
    }

    public void ButtonHit()
    {
        Debug.Log("Button hit!");
        if (currentSelection == currentMouseOver)
            ConfirmSelection(currentSelection);
    }

    private void OnMouseOver()
    {
        
    }

    
}
