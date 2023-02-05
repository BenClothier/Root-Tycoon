using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Transform canvasCamera;
    [SerializeField] private GameObject demandRootPrefab;
    [SerializeField] private GameObject rootButtonPrefab;

    [SerializeField] private RectTransform selectionBox;

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

        Vector3 offsetFromCamera = (canvasCamera.forward * 2f) - (canvasCamera.right * 0.4f) - (canvasCamera.up * 0.65f);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) continue;

            Vector3 individualOffset = canvasCamera.right * i * 0.25f;

            Vector3 position = canvasCamera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, rotation);
            rootObjects[i].transform.parent = canvasCamera;

            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(PlayerStats.GetInventoryItem(i));

            int index = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { ToggleSelection(index); };
        }
    }

    public void UpdateDemand () {
        DeleteDemandObjects();

        Vector3 offsetFromCamera = (canvasCamera.forward * 2f) + (canvasCamera.right * 0.975f) + (canvasCamera.up * 0.4f);

        List<Market.Demand> demands = GameHandler.Market.GetMostRelevantDemands();

        for (int i = 0; i < demands.Count; i++) {
            Vector3 individualOffset = -canvasCamera.up * i * 0.25f;

            Vector3 position = canvasCamera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = canvasCamera.rotation * Quaternion.Euler(0, 0, -20);

            demandObjects[i] = Instantiate(demandRootPrefab, position, rotation);
            demandObjects[i].transform.parent = canvasCamera;

            demandObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(demands[i].DemandedRootAttributes);
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
