using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataUtility : MonoBehaviour, IDataPersistence
{
    //Private
    GameManager creditReference;

    [SerializeField] private List<Sprite> itemSprites;
    private List<string> randomNames;
    private SerializableDictionary<string, int> ItemLevels;
    private static readonly SerializableDictionary<string, string> ItemBlurbs = 
        new SerializableDictionary<string, string> { 
            { "Reinforced Glass", "Planes of glass thick enough to survive space. Take less damage."}, 
            { "Medkit", "Robots need TLC too. Enemies have a chance to drop healing cyrstal on defeat." }, 
            { "Battery", "Increase your energy output with this one simple trick. Flat damage increase for all attacks." }, 
            { "Magnifier", "Universal magnification for all things. Increases attacks size." }, 
            { "Magnet", "Magnetizes your chasey.  Increase pickup range." } };
    private List<string> ItemNames;
    private SerializableDictionary<string, int> EquipmentLevels;
    private static readonly SerializableDictionary<string, string> EquipmentBlurbs =
        new SerializableDictionary<string, string> {
            { "Sword", "It says that its not a real sword, but dont tell your enemies that."},
            { "Laser Pointer", "Given enough energy, you can cause damage with light!" },
            { "Portable Plug", "Bluetooth plug gives you energy to move faster! Ahh, the wonders of technology!" } };
    private List<string> EquipmentNames;
    private SerializableDictionary<string, int> EquipmentCosts;

    private void Awake()
    {
        creditReference = GameObject.Find("GameManager").GetComponent<GameManager>();
        randomNames = new List<string>();

        // Instead of this, just load saved gamedata from the DataManager?
        ItemLevels = new SerializableDictionary<string, int>();
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

        EquipmentLevels = new SerializableDictionary<string, int>();
        EquipmentLevels.Add("Sword", 0);
        EquipmentLevels.Add("Laser Pointer", 0);
        EquipmentLevels.Add("Portable Plug", 0);

        EquipmentNames = new List<string>();
        EquipmentNames.Add("Sword");
        EquipmentNames.Add("Laser Pointer");
        EquipmentNames.Add("Portable Plug");

        EquipmentCosts = new SerializableDictionary<string, int>();
        EquipmentCosts.Add("Sword", 1000);
        EquipmentCosts.Add("Laser Pointer", 1000);
        EquipmentCosts.Add("Portable Plug", 1000);

        LevelUpUtility.OnItemSelected += LevelUpItem;
        randomizer();
    }
    private void OnDestroy()
    {
        LevelUpUtility.OnItemSelected -= LevelUpItem;
    }

    void randomizer()
    {
        if (randomNames.Count != 0)
            randomNames.Clear();

        int temp = Random.Range(0, ItemNames.Count);
        randomNames.Add(ItemNames[temp]);
        while (ItemNames[temp] == randomNames[0])

        {
            temp = Random.Range(0, ItemNames.Count);
        }
        randomNames.Add(ItemNames[temp]);
        while (ItemNames[temp] == randomNames[1] || ItemNames[temp] == randomNames[0])
        {
            temp = Random.Range(0, ItemNames.Count);
        }
        randomNames.Add(ItemNames[temp]);
        //Debug.Log("Indices (names):" + names[0]);
        //Debug.Log("Indices (names):" + names[2]);
    }
    private void LevelUpItem(string name)
    {
        if (ItemLevels.ContainsKey(name))
        {
            ItemLevels[name]++;
            OnDataChange?.Invoke(name, ItemLevels[name]);
        }
        else
        {
            if (creditReference.GetCurrentCredits() >= EquipmentCosts[name])
            {
                EquipmentLevels[name]++;
                OnDataChange?.Invoke(name, EquipmentLevels[name]);
                creditReference.AddCredit(-EquipmentCosts[name]);
            }
            else
            {
                NotEnoughCredits?.Invoke("Not Enough Credits", EquipmentCosts[name]);
            }
        }

    }

    //Public
    public delegate void DataChangeHandler(string changedData, int newLevel);
    public delegate void NotEnoughCreditsHandler(string notEnough, int neededCredits);

    public static event DataChangeHandler OnDataChange;
    public static event NotEnoughCreditsHandler NotEnoughCredits;

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
    public Sprite GetSpriteByName(string name)
    {
        for (int i = 0; i < itemSprites.Count; i++)
        {
            if (itemSprites[i].name == name)
                return itemSprites[i];
        }
        return null;
    }
    public List<string> RandomItemIndices()
    {
        Invoke("randomizer", 1);
        return randomNames;
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

    void IDataPersistence.LoadData(GameData data)
    {
        EquipmentLevels = data.equipmentLevels;
    }
    void IDataPersistence.SaveData(ref GameData data)
    {
        data.equipmentLevels = EquipmentLevels;
    }
}
