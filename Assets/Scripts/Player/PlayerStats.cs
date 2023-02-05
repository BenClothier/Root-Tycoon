using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public const int INVENTORY_SIZE = 4;

    static RootAttributes[] inventory = new RootAttributes[INVENTORY_SIZE] {null, null, null, RootAttributes.Default()}; // Can only keep 6 roots in the inventory
    public static int money = 0;
    public static event Action OnMoneyChange;

    public static event Action OnInventoryChange;

    /// <summary>
    /// Adds a root to the inventory.
    /// </summary>
    /// <param name="rootAttributes">Root being stored</param>
    /// <returns>True if successfully added, false if inventory is full</returns>
    public static bool AddToInventory (RootAttributes rootAttributes) {
        for (int i = 0 ; i < inventory.Length; i++) {
            if (inventory[i] == null) {
                inventory[i] = rootAttributes;
                OnInventoryChange?.Invoke();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes a root from the inventory.
    /// </summary>
    /// <param name="rootAttributes">Root to remove</param>
    /// <returns>True if successfulyl removed, false if the root is not in the inventory</returns>
    public static bool RemoveFromInventory (RootAttributes rootAttributes) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == rootAttributes) {
                inventory[i] = null;
                OnInventoryChange?.Invoke();
                return true;
            }
        }
        return false;
    }

    public static RootAttributes GetInventoryItem (int index) {
        if (index < 0 || index >= INVENTORY_SIZE) return null;

        return inventory[index];
    }

    public static void AddMoney (int amount) {
        money += amount;
        OnMoneyChange?.Invoke();
    }

    public static bool RemoveMoney (int amount) {
        if (money - amount < 0) return false;

        money -= amount;

        OnMoneyChange?.Invoke();
        return true;
    }
}
