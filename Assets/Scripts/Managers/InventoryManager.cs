using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Item[] startItems;
    public Item[] itemList;
    public int maxStackedItems;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventoryButton;
    public Dictionary<Item.ItemType, Item> itemDictionary = new Dictionary<Item.ItemType, Item>();
    public Canvas canvas;
    public GameObject mainInventory;



    int selectedSlot = -1;
    private Dictionary<int, ItemInfo> inventoryStructure = new Dictionary<int, ItemInfo>();

    private struct ItemInfo
    {
        public Item.ItemType itemType;
        public int count;
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    private void Start()
    {
        FillItemDictionary();
        maxStackedItems = 24;
        instance = this;
        ChangeSelectedSlot(0);
        if (!GlobalVariables.ResumeGame)
        {
            foreach (Item item in startItems)
            {
                if (item.stackable)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        AddItem(item);
                    }
                }
                else
                {
                    AddItem(item);
                }
            }
        }
        else
        {
            LoadInventory();
        }
        TimeManager.OnCycleComplete += SaveInventory;
        // la carga del inventario debe esperar a que la interfaz est� completamente cargada
        StartCoroutine(DelayInventoryUpdate());
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public bool AddItem(Item item)
    {
        // compruba si hay algun item stackeable con menos unidades del maximo
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }

    private void FillItemDictionary()
    {
        foreach (Item item in itemList)
        {
            itemDictionary.Add(item.type, item);
        }
    }

    private void SaveInventory()
    {
        ItemInfo itemInfo;
        InventoryItem item;
        string itemLine;
        try
        {
            inventoryStructure.Clear();
            // guardar la estructura del inventario
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if ((item = inventorySlots[i].GetComponentInChildren<InventoryItem>()) != null)
                {
                    itemInfo.itemType = item.item.type;
                    itemInfo.count = item.count;
                    inventoryStructure.Add(i, itemInfo);
                }
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine(Application.persistentDataPath, "inventorydata.fmout"), false))
            {
                foreach (KeyValuePair<int, ItemInfo> keyValuePair in inventoryStructure)
                {
                    // posicion del slot, tipo y cantidad
                    itemLine = $"{keyValuePair.Key.ToString()};{keyValuePair.Value.itemType.ToString()};{keyValuePair.Value.count}";
                    sw.WriteLine(itemLine);
                }
            }
            Debug.Log($"Inventario guardado en {Path.Combine(Application.persistentDataPath, "inventorydata.fmout")}");
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Error al guardar el invetario {ex.Message}");
        }
    }

    private void LoadInventory()
    {
        string itemLine;
        string[] itemValues;
        ItemInfo itemInfo;
        try
        {
            using (StreamReader sr = new StreamReader(Path.Combine(Application.persistentDataPath, "inventorydata.fmout")))
            {
                while ((itemLine = sr.ReadLine()) != null)
                {
                    if (itemLine.Trim() != "")
                    {
                        itemValues = itemLine.Split(';');
                        int index = int.Parse(itemValues[0]);
                        itemInfo.itemType = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), itemValues[1]);
                        itemInfo.count = int.Parse(itemValues[2]);

                        inventoryStructure.Add(index, itemInfo);
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log($"Error al cargar el invetario {ex.Message}");
        }
    }

    IEnumerator DelayInventoryUpdate()
    {
        yield return new WaitForEndOfFrame();

        UpdateInventory();
    }

    private void UpdateInventory()
    {
        Vector3 slotPosition;
        Item item;
        foreach (KeyValuePair<int, ItemInfo> pair in inventoryStructure)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (i == pair.Key) // debido a la imprecision de float necesito una tolerancia m�s baja para comparar
                {
                    item = GetCorrectItem(pair.Value.itemType);
                    SpawnNewItem(item, inventorySlots[i]);
                    for (int j = 0; j < pair.Value.count - 1; j++)
                    {
                        AddItem(item);
                    }
                }
            }
        }
    }

    private Item GetCorrectItem(Item.ItemType type)
    {
        foreach (Item item in itemList)
        {
            if (item.type == type)
            {
                return item;
            }
        }
        return null;
    }
}
