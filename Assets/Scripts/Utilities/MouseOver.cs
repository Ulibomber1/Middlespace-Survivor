using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MouseOver : MonoBehaviour, IPointerEnterHandler
{
    public delegate void DescriptionHandler(string name, bool isAddable);
    public static event DescriptionHandler OnItemMouseover;

    [SerializeField] private string itemName;
    [SerializeField] private bool isAddable = false;

    private void Awake()
    {
        // 
        // set itemName to the itemName string in data

    }

    public void SetupButton(string name)
    {
        itemName = name;
        gameObject.GetComponentInChildren<TextMeshPro>().text = name;
        // We could also change image of button instead
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // set the blurb (call the dialog system)
        OnItemMouseover?.Invoke(itemName, isAddable);
    }

}
