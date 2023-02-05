using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject harvestPanel;
    [SerializeField] private GameObject rootButtonPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowBarPrefab;

    GameObject[] rootObjects = new GameObject[20];

    GameObject[] inventoryRoots = new GameObject[4];

    GameObject[] arrows = new GameObject[20];

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

        for (int i = 0; i < arrows.Length; i++) {
            if (arrows[i] == null) continue;
            Destroy(arrows[i]);
        }
    }
    
    private void DestroyInventoryRoots () {
        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (inventoryRoots[i] == null) continue;
            Destroy(inventoryRoots[i]);
        }
    }

    public void SetupUI (List<RootAttributes> rootAttributes) {
        this.rootAttributes = rootAttributes;
        UpdateHarvest();
        UpdateInventory();
    }

    public void UpdateHarvest () {
        DestroyHarvestRoots();

        Vector3 offsetFromCamera = (camera.forward * 2f) - (camera.right * 1f) - (camera.up * 0.35f);

        for (int i = 0; i < rootAttributes.Count; i++) {
            int xPos = i % 5;
            int yPos = i / 5;

            Vector3 individualOffset = (camera.right * xPos * 0.3f) + (0.25f * camera.up * yPos);

            Vector3 position = camera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = camera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, rotation);
            rootObjects[i].transform.parent = camera;
            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(rootAttributes[i]);

            int ix = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { HarvestRootButton(ix); };

            int salePrice = GameHandler.Market.GetSalePriceOfRoot(rootAttributes[i]); // Get the price of the current root
            int originalSalePrice = GameHandler.Market.GetSalePriceOfRoot(rootAttributes[0]);
            if (salePrice < originalSalePrice) {
                // Red down arrow
                arrows[i] = Instantiate(arrowPrefab, position + (camera.right * 0.135f), camera.rotation);
                arrows[i].transform.Rotate(new Vector3(0, 0, 180));
                arrows[i].GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }
            else if (salePrice > originalSalePrice) {
                // Green up arrow
                arrows[i] = Instantiate(arrowPrefab, position + (camera.right * 0.135f), camera.rotation);
                arrows[i].GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            }
            else {
                // TODO bar
                arrows[i] = Instantiate(arrowBarPrefab, position + (camera.right * 0.135f), camera.rotation);
            }
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

        CanvasManager.IsHarvestActive = false;

        gameUI.SetActive(true);
    }
}
