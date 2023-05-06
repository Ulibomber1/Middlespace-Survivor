using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemDataUtility : MonoBehaviour
{
    private Dictionary<string, int> ItemLevels;
    private static readonly Dictionary<string, string> ItemBlurbs = 
        new Dictionary<string, string> { 
            { "Reinforced Glass", "Planes of glass thick enough to survive space. Take less damage."}, 
            { "Medkit", "Robots need TLC too. Enemies have a chance to drop healing cyrstal on defeat." }, 
            { "Battery", "Increase your energy output with this one simple trick. Flat damage increase for all attacks." }, 
            { "Magnifier", "Universal magnification for all things. Increases attacks size." }, 
            { "Magnet", "Magnetizes your chasey.  Increase pickup range." } };
    private List<string> ItemNames;

    private Dictionary<string, int> EquipmentLevels;
    private static readonly Dictionary<string, string> EquipmentBlurbs =
        new Dictionary<string, string> {
            { "Sword", "It says that its not a real sword, but dont tell your enemies that."},
            { "Laser Pointer", "Given enough energy, you can cause damage with light!" },
            { "Portable Plug", "Bluetooth plug gives you energy to move faster! Ahh, the wonders of technology!" } };
    private List<string> EquipmentNames;

    private void Awake()
    {
        ItemLevels = new Dictionary<string, int>();
        ItemLevels.Add("Reinforced Glass", 0);
        ItemLevels.Add("Medkit", 0);
        ItemLevels.Add("Battery", 0);
        ItemLevels.Add("Magnifier", 0);
        ItemLevels.Add("Magnet", 0);

        ItemNames = new List<string>();
        ItemNames.Add("Reinforced Glass");
        ItemNames.Add("Medkit");
        ItemNames.Add("Battery");
        ItemNames.Add("Magnifier");
        ItemNames.Add("Magnet");

        EquipmentLevels = new Dictionary<string, int>();
        EquipmentLevels.Add("Sword", 0);
        EquipmentLevels.Add("Laser Pointer", 0);
        EquipmentLevels.Add("Portable Plug", 0);

        EquipmentNames = new List<string>();
        EquipmentNames.Add("Sword");
        EquipmentNames.Add("Laser Pointer");
        EquipmentNames.Add("Portable Plug");

        LevelUpUtility.OnItemSelected += LevelUpItem;
    }

    public delegate void DataChangeHandler(string changedData, int newLevel);
    public static event DataChangeHandler OnDataChange;
    private void LevelUpItem(string name)
    {
        if (ItemLevels.ContainsKey(name))
        {
            ItemLevels[name]++;
            OnDataChange?.Invoke(name, ItemLevels[name]);
        }
        else
        {
            EquipmentLevels[name]++;
            OnDataChange?.Invoke(name, EquipmentLevels[name]);
        }

    }
    public string GetItemBlurb(string name)
    {
        if (ItemLevels.ContainsKey(name))
            return ItemBlurbs[name];
        else
            return EquipmentBlurbs[name];
    }
    public int GetItemLevel(string name)
    {
        if (ItemLevels.ContainsKey(name))
            return ItemLevels[name];
        else
            return EquipmentLevels[name];
    }
    public string GetItemNameByIndex(int index)
    {
        return ItemNames[index];
    }
    public string GetEquipmentNameByIndex(int index)
    {
        return EquipmentNames[index];
    }
    public List<string> RandomItemIndices()
    {
        List<string> names = new List<string>();

        int temp = Random.Range(0, ItemNames.Count - 1);
        names.Add(ItemNames[temp]);
        while (ItemNames[temp] == names[0])
        {
            temp = Random.Range(0, ItemNames.Count - 1);
        }
        names.Add(ItemNames[temp]);
        while (ItemNames[temp] == names[1] || ItemNames[temp] == names[0])
        {
            temp = Random.Range(0, ItemNames.Count - 1);
        }
        names.Add(ItemNames[temp]);
        Debug.Log("Indices (names):" + names[0]);
        Debug.Log("Indices (names):" + names[2]);
        return names;
    }

    public List<string> RandomEquipmentIndices()
    {
        List<string> names = new List<string>();

        int temp = Random.Range(0, EquipmentNames.Count - 1);
        names.Add(EquipmentNames[temp]);
        while (EquipmentNames[temp] == names[0])
        {
            temp = Random.Range(0, EquipmentNames.Count - 1);
        }
        names.Add(EquipmentNames[temp]);

        return names;
    }
}
