using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject rootButtonPrefab;

    [SerializeField] private RectTransform selectionBox;

    GameObject[] rootObjects = new GameObject[4];

    private void OnEnable() {
        UpdateInventory();
        PlayerStats.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable() {
        DeleteInventoryObjects();
        PlayerStats.OnInventoryChange -= UpdateInventory;
    }

    public void UpdateInventory () {
        DeleteInventoryObjects();

        Vector3 offsetFromCamera = (camera.forward * 2f) - (camera.right * 0.4f) - (camera.up * 0.65f);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) continue;

            Vector3 individualOffset = camera.right * i * 0.25f;

            Vector3 position = camera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = camera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, rotation);
            rootObjects[i].transform.parent = camera;

            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(PlayerStats.GetInventoryItem(i));

            int index = i;
            rootObjects[i].GetComponent<HarvestRoot>().OnClick += delegate { ToggleSelection(index); };
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
