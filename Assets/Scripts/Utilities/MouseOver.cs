using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MouseOver : MonoBehaviour, IPointerEnterHandler
{
    public delegate void DescriptionHandler(string name, bool isAddable);
    public static event DescriptionHandler OnItemMouseover;

    private ItemDataUtility itemData;
    [SerializeField] private string itemName;
    [SerializeField] private bool isAddable = false;
    [SerializeField] private TextMeshProUGUI textmesh;

    private void Awake()
    {
        itemData = GetComponentInParent<ItemDataUtility>();

        
    }

    public void SetupButton(string name)
    {
        itemName = name;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
        int itemLevel = itemData.GetItemLevel(name);
        if (isAddable)
            gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Lvl {itemLevel} -> {itemLevel + 1}";
        else
            gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Lvl {itemLevel}";
        // We could also change image of button instead
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // set the blurb (call the dialog system)
        OnItemMouseover?.Invoke(itemName, isAddable);
    }

}
