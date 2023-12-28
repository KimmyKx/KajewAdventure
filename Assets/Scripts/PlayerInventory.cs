using System.Collections;
using System.Collections.Generic;
using DevionGames.InventorySystem;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject itemSlotPanel;
    public List<ItemInfoScriptable> inventory;
    public List<ItemSlot> slots;
    private void Start()
    {
        UIManager.Instance.OnReceiveItem += OnReceiveItem;
        inventory = new List<ItemInfoScriptable>();
    }

    private void OnReceiveItem(ItemInfoScriptable itemInfo)
    {
        if(itemInfo.stackable)
        {
            for(int i = 0; i < inventory.Count; i++)
            {
                if(inventory[i].itemId == itemInfo.itemId)
                {
                    inventory[i].itemCount += itemInfo.itemCount;
                    slots[i].SetCount(inventory[i].itemCount);
                    return;
                }
            }

        }
        string serialized = JsonConvert.SerializeObject(itemInfo);
        inventory.Add(JsonConvert.DeserializeObject<ItemInfoScriptable>(serialized));
        int j = inventory.Count - 1;
        slots[j].SetCount(itemInfo.itemCount);
        slots[j].SetImage(itemInfo.GetItemSprite());
    }
}
