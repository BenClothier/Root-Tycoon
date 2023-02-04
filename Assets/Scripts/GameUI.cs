using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject rootButtonPrefab;

    GameObject[] rootObjects = new GameObject[4];

    private void OnEnable() {
        UpdateInventory();
    }

    private void OnDisable() {
        DeleteInventoryObjects();
    }

    public void UpdateInventory () {
        Vector3 offsetFromCamera = (camera.forward * 2f) - (camera.right * 0.4f) - (camera.up * 0.65f);

        for (int i = 0; i < PlayerStats.INVENTORY_SIZE; i++) {
            if (PlayerStats.GetInventoryItem(i) == null) continue;

            Vector3 individualOffset = camera.right * i * 0.25f;

            Vector3 position = camera.position + offsetFromCamera + individualOffset;
            Quaternion rotation = camera.rotation * Quaternion.Euler(0, 0, -20);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, rotation);
            rootObjects[i].transform.parent = camera;

            rootObjects[i].GetComponentInChildren<RootRenderer>().Inititialise(PlayerStats.GetInventoryItem(i));
        }
    }

    public void DeleteInventoryObjects () {
        foreach (GameObject rootObject in rootObjects) {
            Destroy(rootObject);
        }
    }
}
