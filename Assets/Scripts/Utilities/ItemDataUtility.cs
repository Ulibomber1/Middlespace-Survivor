using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemDataUtility : MonoBehaviour
{
    private Dictionary<string, int> ItemLevels;
    private static readonly Dictionary<string, string> ItemBlurbs = new Dictionary<string, string> { { "Reinforced Glass", "desc"}, { "Medkit", "desc" }, { "Battery", "desc" }, { "Magnifier", "desc" }, { "Magnet", "desc" } };
    private List<string> ItemIndices;

    private void Start()
    {
        ItemLevels.Add("Reinforced Glass", 0);
        ItemLevels.Add("Medkit", 0);
        ItemLevels.Add("Battery", 0);
        ItemLevels.Add("Magnifier", 0);
        ItemLevels.Add("Magnet", 0);

        ItemIndices.Add("Reinforced Glass");
        ItemIndices.Add("Medkit");
        ItemIndices.Add("Battery");
        ItemIndices.Add("Magnifier");
        ItemIndices.Add("Magnet");
        //for (int i = 0; i < Item)
    }

    public string GetItemBlurb(string name)
    {
        return ItemBlurbs[name];
    }

    public int GetItemLevel(string name)
    {
        return ItemLevels[name];
    }

    public List<string> RandomItemIndices()
    {
        List<string> indices = null;

        int temp = Random.Range(0, ItemIndices.Count - 1);
        indices.Add(ItemIndices[temp]);
        while (ItemIndices[temp] == indices[0])
        {
            temp = Random.Range(0, ItemIndices.Count - 1);
        }
        indices.Add(ItemIndices[temp]);
        while (ItemIndices[temp] == indices[1] || ItemIndices[temp] == indices[0])
        {
            temp = Random.Range(0, ItemIndices.Count - 1);
        }
        indices.Add(ItemIndices[temp]);

        return indices;
    }
}
