using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    [SerializeField] private Transform canvasCamera;
    [SerializeField] private GameObject harvestPanel;
    [SerializeField] private GameObject rootButtonPrefab;

    GameObject[] rootObjects = new GameObject[20];

    GameObject[] inventoryRoots = new GameObject[4];

    RootAttributes originalRoot;
    List<RootAttributes> rootAttributes;

    [SerializeField] private GameObject gameUI;

    private void OnDisable() {
        DestroyHarvestRoots();
        DestroyInventoryRoots();
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

    public void SetupUI (RootAttributes originalRoot, List<RootAttributes> rootAttributes) {
        this.originalRoot = originalRoot;
        this.rootAttributes = rootAttributes;
        UpdateHarvest();
        UpdateInventory();
    }

    public void UpdateHarvest () {
        DestroyHarvestRoots();

        Vector3 startPos = new Vector3(-2.75f,1.14999962f,9);

        for (int i = 0; i < rootAttributes.Count; i++) {
            int xPos = i % 5;
            int yPos = i / 5;

            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            Vector3 localPos = startPos + (new Vector3(xPos, -yPos * 0.85f, 0) * 0.85f);

            rootObjects[i] = Instantiate(rootButtonPrefab, Vector3.zero, rotation);
            rootObjects[i].transform.parent = canvasCamera;
            rootObjects[i].transform.localPosition = localPos;
            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(rootAttributes[i]);

            int ix = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { HarvestRootButton(ix); };

            int salePrice = GameHandler.Market.GetSalePriceOfRoot(rootAttributes[i]); // Get the price of the current root
            int originalSalePrice = GameHandler.Market.GetSalePriceOfRoot(originalRoot);
        }
    }

    public void UpdateInventory () {
        DestroyInventoryRoots();

        Vector3 startPos = new Vector3(2.41f, 1.14999962f, 9);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) continue;

            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            Vector3 localPos = startPos + (new Vector3(0, -i * 0.85f, 0) * 0.85f);

            inventoryRoots[i] = Instantiate(rootButtonPrefab, Vector3.zero, rotation);
            inventoryRoots[i].transform.parent = canvasCamera;
            inventoryRoots[i].transform.localPosition = localPos;
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

        CanvasManager.IsHarvestActive = false;

        gameUI.SetActive(true);
    }
}
