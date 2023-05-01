using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemDataUtility : MonoBehaviour
{
    private Dictionary<string, int> ItemLevels;
    private static readonly Dictionary<string, string> ItemBlurbs = new Dictionary<string, string> { { "Reinforced Glass", "desc"}, { "Medkit", "desc" }, { "Battery", "desc" }, { "Magnifier", "desc" }, { "Magnet", "desc" } };
    private List<string> ItemNames;

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
        //for (int i = 0; i < Item)

        LevelUpUtility.OnItemSelected += LevelUpItem;
    }

    public delegate void DataChangeHandler(string changedData, int newLevel);
    public static event DataChangeHandler OnDataChange;
    private void LevelUpItem(string name)
    {
        ItemLevels[name]++;
        OnDataChange?.Invoke(name, ItemLevels[name]);
    }
    public string GetItemBlurb(string name)
    {
        return ItemBlurbs[name];
    }
    public int GetItemLevel(string name)
    {
        return ItemLevels[name];
    }
    public string GetItemNameByIndex(int index)
    {
        return ItemNames[index];
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
}
