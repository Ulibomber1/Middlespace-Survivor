using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int runsSurvived;
    public int currentCredits;
    public int totalCredits;
    public SerializableDictionary<string, int> itemLevels;
    public SerializableDictionary<string, int> equipmentLevels;

    public GameData()
    {
        runsSurvived = 0;

        currentCredits = 0;

        totalCredits = 0;

        itemLevels = new SerializableDictionary<string, int>();
        itemLevels.Add("Reinforced Glass", 0);
        itemLevels.Add("Medkit", 0);
        itemLevels.Add("Battery", 0);
        itemLevels.Add("Magnifier", 0);
        itemLevels.Add("Magnet", 0);

        equipmentLevels = new SerializableDictionary<string, int>();
        equipmentLevels.Add("Sword", 0);
        equipmentLevels.Add("Laser Pointer", 0);
        equipmentLevels.Add("Portable Plug", 0);
    }
}
