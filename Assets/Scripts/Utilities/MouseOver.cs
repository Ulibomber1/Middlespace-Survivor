using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Global Enums

public class MouseOver : MonoBehaviour, IPointerEnterHandler
{
    // Private Enums

    // References
    private ItemDataUtility itemData;
    [SerializeField] private TextMeshProUGUI textmesh;
    [SerializeField] private Sprite itemSprite;

    // Internal Variables
    [SerializeField] private string itemName;
    [SerializeField] private bool isAddable = false;

    // User-defined Objects
    
    // Delegates
    public delegate void DescriptionHandler(string name, bool isAddable);

    // Events
    public static event DescriptionHandler OnItemMouseover;

    // Unity Methods
    private void Awake()
    {
        itemData = GetComponentInParent<ItemDataUtility>();
        itemSprite = GetComponentsInChildren<Image>()[0].sprite;
    }

    // User-Defined Methods
    public void SetupButton(string name)
    {
        itemName = name;

        int itemLevel = itemData.GetItemLevel(name);
        if (isAddable)
        {
            gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Lvl {itemLevel} -> {itemLevel + 1}";
            GetComponentsInChildren<Image>()[1].sprite = itemData.GetSpriteByName(name);
        }
        else
            gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"Lvl {itemLevel}";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // set the blurb (call the dialog system)
        OnItemMouseover?.Invoke(itemName, isAddable);
    }
}
