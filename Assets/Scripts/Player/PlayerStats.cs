using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public const int INVENTORY_SIZE = 6;

    RootAttributes[] inventory = new RootAttributes[INVENTORY_SIZE]; // Can only keep 6 roots in the inventory

    /// <summary>
    /// Adds a root to the inventory.
    /// </summary>
    /// <param name="rootAttributes">Root being stored</param>
    /// <returns>True if successfully added, false if inventory is full</returns>
    public bool AddToInventory (RootAttributes rootAttributes) {
        for (int i = 0 ; i < inventory.Length; i++) {
            if (inventory[i] == null) {
                inventory[i] = rootAttributes;
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
    public bool RemoveFromInventory (RootAttributes rootAttributes) {
        for (int i = 0; i < inventory.Length; i++) {
            if (inventory[i] == rootAttributes) {
                inventory[i] = null;
                return true;
            }
        }
        return false;
    }

    public RootAttributes GetInventoryItem (int index) {
        if (index < 0 || index >= INVENTORY_SIZE) return null;

        return inventory[index];
    }
}
