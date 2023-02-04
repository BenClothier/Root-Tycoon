using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject harvestPanel;
    [SerializeField] private GameObject rootButtonPrefab;

    GameObject[] rootObjects = new GameObject[16];

    public static event Action<List<RootRenderer>> OnHarvest;
    public static void HarvestRoots (List<RootRenderer> rootTransforms) {
        OnHarvest?.Invoke(rootTransforms);
    }

    private void OnEnable() {
        OnHarvest += moveRoots;
    }

    private void OnDisable() {
        OnHarvest -= moveRoots;
    }

    public void moveRoots (List<RootRenderer> rootRenderers) {
        // Enable Havest UI Panel
        harvestPanel.SetActive(true);

        // Move roots onto the UI
        for (int i = 0; i < rootRenderers.Count; i++) {
            float xIndex = (i % 4) - 1.5f;
            float yIndex = (i / 4) - 1.5f;

            // Move to camera
            Vector3 offset = (playerCamera.right * xIndex * 0.35f) + (playerCamera.up * yIndex * 0.45f);
            Vector3 position = (playerCamera.position + playerCamera.forward * 2.75f) + offset;

            // Look towards the camera
            Vector3 direction = (playerCamera.position - rootRenderers[i].transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            rootObjects[i] = Instantiate(rootButtonPrefab, position, lookRotation);
        }
    }

    public void DisableUI () {
        // Disable harvest UI panel
        harvestPanel.SetActive(false);

        // Destroy all roots on UI
        foreach (GameObject rootObject in rootObjects) {
            Destroy(rootObject);
        }
    }
}
