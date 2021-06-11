using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item this_Item;
    public Inventory player_Inventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int weaponToEnable = -1;
            if (this_Item.item_Name == "Shotgun") weaponToEnable = 1;
            if (this_Item.item_Name == "Rifle") weaponToEnable = 2;
            other.gameObject.transform.Find("WeaponComponent")?.GetComponent<weaponsManager>()?.setWeaponAvailability(weaponToEnable, true);
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if (!player_Inventory.itemList.Contains(this_Item))
        {
            player_Inventory.itemList.Add(this_Item);
            this_Item.item_Held = 1;
        }
        else
        {
            this_Item.item_Held += 1;
        }
        InventoryManager.RefreshItem();
    }
}