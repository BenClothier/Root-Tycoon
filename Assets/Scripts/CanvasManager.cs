using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static bool IsHarvestActive = false;

    public static event Action<List<RootAttributes>> OnHarvest;

    [SerializeField] private GameUI gameUI;
    [SerializeField] private HarvestUI harvestUI;

    private static List<RootAttributes> rootsHeld;

    public static void HarvestRoots (RootAttributes originalRoot, List<RootAttributes> rootAttributes) {
        _instance.harvestUI.gameObject.SetActive(true);
        _instance.gameUI.gameObject.SetActive(false);
        _instance.harvestUI.SetupUI(originalRoot, rootAttributes);
        rootsHeld = rootAttributes;
    }

    private static CanvasManager _instance;

    private void Awake() {
        _instance = this;
    }

    public void CloseHarvestUI () {
        _instance.harvestUI.gameObject.SetActive(false);
        _instance.gameUI.gameObject.SetActive(true);
        GameHandler.Market.SellAll(rootsHeld);
    }
}
