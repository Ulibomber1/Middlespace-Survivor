using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Global Enums

public class LevelUpUtility : MonoBehaviour
{
    // Private Enums

    // References

    // Internal Variables
    [SerializeField] private string currentSelection = null;
    [SerializeField] private string currentMouseOver = null;

    // User-Defined Objects

    // Delegates
    public delegate void LevelUpGUIHandler(GameObject gui);
    public delegate void ItemSelectedHandler(string itemName);
    
    // Events
    public static event LevelUpGUIHandler OnAwake;
    public static event ItemSelectedHandler OnItemSelected;

    // Unity Methods
    private void Awake()
    {
        OnAwake?.Invoke(gameObject);
        MouseOver.OnItemMouseover += ChangeItemSelection;
        GameManager.Instance.OnDataReady += InputItemData;
    }
    private void OnDestroy()
    {
        MouseOver.OnItemMouseover -= ChangeItemSelection;
        GameManager.Instance.OnDataReady -= InputItemData;
    }
    private void OnMouseOver()
    {

    }
    
    // User-Defined Methods
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
        OnItemSelected?.Invoke(selection);
    }
    public void ButtonHit() //called by event system gameobject
    {
        Debug.Log("Button hit!");
        if (currentSelection == currentMouseOver)
            ConfirmSelection(currentSelection);
    }
}
