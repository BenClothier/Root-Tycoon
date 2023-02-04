using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject harvestPanel;
    [SerializeField] private GameObject rootButtonPrefab;

    GameObject[] rootObjects = new GameObject[20];

    GameObject[] inventoryRoots = new GameObject[4];

    public static bool IsPanelActive = false;

    List<RootAttributes> rootAttributes;

    [SerializeField] private GameObject gameUI;

    public static event Action<List<RootAttributes>> OnHarvest;
    public static void HarvestRoots (List<RootAttributes> rootAttributes) {
        OnHarvest?.Invoke(rootAttributes);
    }

    void Awake () {
        OnHarvest += setupUI;
    }

    private void DestroyHarvestRoots () {
        for (int i = 0; i < rootObjects.Length; i++) {
            if (rootObjects[i] == null) continue;
            Destroy(rootObjects[i]);
        }
    }
    
    private void DestroyInventoryRoots () {
        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (inventoryRoots[i] == null) continue;
            Destroy(inventoryRoots[i]);
        }
    }

    public void setupUI (List<RootAttributes> rootAttributes) {
        gameUI.SetActive(false);

        IsPanelActive = true;

        // Enable Havest UI Panel
        harvestPanel.SetActive(true);

        this.rootAttributes = rootAttributes;
        UpdateHarvest();
    }

    public void UpdateHarvest () {
        DestroyHarvestRoots();

        Vector3 offsetFromCamera = (camera.forward * 2f) - (camera.right * 0.85f) - (camera.up * 0.35f);

        for (int i = 0; i < rootAttributes.Count; i++) {
            int xPos = i % 5;
            int yPos = i / 5;

            Vector3 individualOffset = (camera.right * xPos + camera.up * yPos) * 0.25f;

            Vector3 position = camera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = camera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, rotation);
            rootObjects[i].transform.parent = camera;
            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(rootAttributes[i]);

            int ix = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { HarvestRootButton(ix); };
        }
    }

    public void UpdateInventory () {
        DestroyInventoryRoots();

        Vector3 offsetFromCamera = (camera.forward * 2f) + (camera.right * 0.8f) - (camera.up * 0.35f);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) continue;

            Vector3 individualOffset = (camera.up * i) * 0.25f;

            Vector3 position = camera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = camera.rotation * Quaternion.Euler(0, 0, -20);

            inventoryRoots[i] = Instantiate(rootButtonPrefab, position, rotation);
            inventoryRoots[i].transform.parent = camera;
            inventoryRoots[i].GetComponentInChildren<RootRenderer>().Inititialise(PlayerStats.GetInventoryItem(i));

            int ix = i;
            inventoryRoots[i].GetComponent<HarvestRoot>().OnClick += delegate { InventoryRootButton(ix); };
        }
    }

    public void InventoryRootButton (int inventoryIndex) {
        RootAttributes ra = inventoryRoots[inventoryIndex].GetComponentInChildren<RootRenderer>().GetAttributes();
        rootAttributes.Add(ra);
        PlayerStats.RemoveFromInventory(PlayerStats.GetInventoryItem(inventoryIndex)); 

        UpdateHarvest();
        UpdateInventory();
    }

    public void HarvestRootButton (int harvestIndex) {
        bool success = PlayerStats.AddToInventory(rootAttributes[harvestIndex]);
        if (!success) return;

        rootAttributes.Remove(rootAttributes[harvestIndex]);
        UpdateHarvest();
        UpdateInventory();
    }

    public void DisableUI () {
        // Disable harvest UI panel
        harvestPanel.SetActive(false);

        DestroyHarvestRoots();
        DestroyInventoryRoots();

        IsPanelActive = false;

        gameUI.SetActive(true);
    }
}
