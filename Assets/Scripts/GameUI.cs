using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Transform canvasCamera;
    [SerializeField] private GameObject demandRootPrefab;
    [SerializeField] private GameObject rootButtonPrefab;

    [SerializeField] private RectTransform selectionBox;

    [Space]
    [SerializeField] private TextMeshProUGUI[] demandTexts;
    [SerializeField] private TextMeshProUGUI[] inventoryTexts;

    GameObject[] rootObjects = new GameObject[4];

    GameObject[] demandObjects = new GameObject[3];

    private void OnEnable() {
        UpdateInventory();
        UpdateDemand();
        PlayerStats.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable() {
        DeleteInventoryObjects();
        DeleteDemandObjects();
        PlayerStats.OnInventoryChange -= UpdateInventory;
    }

    public void UpdateInventory () {
        DeleteInventoryObjects();

        Vector3 startPos = new Vector3(-1.12f,-1.675f,9);

        Vector3 textPos = new Vector3(-200, 0, 0);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) {
                inventoryTexts[i].gameObject.SetActive(false);
                continue;
            }
            else {
                inventoryTexts[i].gameObject.SetActive(true);
            }

            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, Vector3.zero, rotation);
            rootObjects[i].transform.parent = canvasCamera;
            rootObjects[i].transform.localPosition = startPos + (i * new Vector3(0.735f, 0, 0));

            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(PlayerStats.GetInventoryItem(i));

            int index = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { ToggleSelection(index); };

            int salePrice = GameHandler.Market.GetSalePriceOfRoot(PlayerStats.GetInventoryItem(i)); // Get the price of the current root
            inventoryTexts[i].text = "$" + salePrice;
            inventoryTexts[i].color = new Color(226, 226, 226 );
        }
    }

    public void UpdateDemand () {
        DeleteDemandObjects();

        Vector3 startPos = new Vector3(3.02999997f,1.34000015f,9);
        Vector3 startText = new Vector3(0, 90, 0);

        List<Market.Demand> demands = GameHandler.Market.GetMostRelevantDemands();

        for (int i = 0; i < demands.Count; i++) {
            // Vector3 position = canvasCamera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            demandObjects[i] = Instantiate(demandRootPrefab, Vector3.zero, rotation);
            demandObjects[i].transform.parent = canvasCamera;
            demandObjects[i].transform.localPosition = startPos + (i * new Vector3(0, -0.65f, 0));

            demandObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(demands[i].DemandedRootAttributes);

            int salePrice = GameHandler.Market.GetSalePriceOfRoot(demands[i].DemandedRootAttributes); // Get the price of the current root
            demandTexts[i].text = "$" + salePrice;
            demandTexts[i].color = new Color(226, 226, 226 );
        }

        if (demands.Count == 1) {
            demandTexts[0].gameObject.SetActive(true);
            demandTexts[1].gameObject.SetActive(false);
            demandTexts[2].gameObject.SetActive(false);
        }
        if (demands.Count == 2) {
            demandTexts[0].gameObject.SetActive(true);
            demandTexts[1].gameObject.SetActive(true);
            demandTexts[2].gameObject.SetActive(false);
        }
        if (demands.Count == 3) {
            demandTexts[0].gameObject.SetActive(true);
            demandTexts[1].gameObject.SetActive(true);
            demandTexts[2].gameObject.SetActive(true);
        }
    }

    public void DeleteDemandObjects () {
        foreach (GameObject rootObject in demandObjects) {
            Destroy(rootObject);
        }
    }

    public void DeleteInventoryObjects () {
        GameHandler.CurrentSelection = -1;
        selectionBox.gameObject.SetActive(false);

        foreach (GameObject rootObject in rootObjects) {
            Destroy(rootObject);
        }
    }

    public void ToggleSelection (int index) {
        if (GameHandler.CurrentSelection == index) {
            GameHandler.CurrentSelection = -1;
            selectionBox.gameObject.SetActive(false);
        }
        else {
            GameHandler.CurrentSelection = index;
            selectionBox.localPosition = new Vector3(190.3529f * index - (190.3529f * 1.5f), selectionBox.localPosition.y, selectionBox.localPosition.z);
            selectionBox.gameObject.SetActive(true);
        }
    }

}
