using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject harvestPanel;
    [SerializeField] private GameObject rootButtonPrefab;

    GameObject[] rootObjects = new GameObject[16];

    GameObject[] inventoryRoots = new GameObject[6];

    public static bool IsPanelActive = false;

    List<RootRenderer> rootRenderers;

    public static event Action<List<RootRenderer>> OnHarvest;
    public static void HarvestRoots (List<RootRenderer> rootTransforms) {
        OnHarvest?.Invoke(rootTransforms);
    }

    private void OnEnable() {
        OnHarvest += setupUI;
        OnHarvest += _ => UpdateInventory();
    }

    private void OnDisable() {
        OnHarvest -= setupUI;
        OnHarvest -= _ => UpdateInventory();
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

    public void setupUI (List<RootRenderer> rootRenderers) {
        IsPanelActive = true;

        // Enable Havest UI Panel
        harvestPanel.SetActive(true);

        this.rootRenderers = rootRenderers;
        UpdateHarvest();
    }

    public void UpdateHarvest () {
        DestroyHarvestRoots();

        // Move roots onto the UI
        for (int i = 0; i < rootRenderers.Count; i++) {
            float xIndex = (i % 4) - 1.5f;
            float yIndex = (i / 4) - 1.5f;

            // Move to camera
            Vector3 offset = (playerCamera.right * xIndex * 0.3f) + (playerCamera.up * (yIndex + 0.5f) * 0.3f);
            Vector3 position = (playerCamera.position + playerCamera.forward * 2.75f) + offset;

            // Create the root button objects
            rootObjects[i] = Instantiate(rootButtonPrefab, position, playerCamera.rotation);
            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(rootRenderers[i].GetAttributes());

            int index = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate{ AttemptAddToInventory(index); };
        }
    }

    public void UpdateInventory () {
        DestroyInventoryRoots();

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (playerStats.GetInventoryItem(i) == null) continue;

            float index = i - 2.5f;
            Vector3 offset = (playerCamera.right * index * 0.35f) + (playerCamera.up * -0.75f);
            Vector3 position = (playerCamera.position + playerCamera.forward * 2.75f) + offset;

            inventoryRoots[i] = Instantiate(rootButtonPrefab, position, playerCamera.rotation);
            inventoryRoots[i].GetComponentInChildren<RootRenderer>().Inititialise(playerStats.GetInventoryItem(i));
            int ix = i;
            inventoryRoots[i].GetComponent<HarvestRoot>().OnClick += delegate { 
                playerStats.RemoveFromInventory(playerStats.GetInventoryItem(ix)); 
                UpdateInventory(); 
            };

        }
    }

    public void AttemptAddToInventory (int harvestIndex) {
        bool success = playerStats.AddToInventory(rootRenderers[harvestIndex].GetAttributes());
        if (!success) return;

        rootRenderers.Remove(rootRenderers[harvestIndex]);
        UpdateHarvest();
        UpdateInventory();
    }

    public void SellInventoryRoot (int inventoryIndex) {
        bool success = playerStats.RemoveFromInventory(playerStats.GetInventoryItem(inventoryIndex)); 
        if (!success) return;

        UpdateHarvest();
        UpdateInventory();
    }

    public void DisableUI () {
        // Disable harvest UI panel
        harvestPanel.SetActive(false);

        DestroyHarvestRoots();
        DestroyInventoryRoots();

        IsPanelActive = false;
    }
}
