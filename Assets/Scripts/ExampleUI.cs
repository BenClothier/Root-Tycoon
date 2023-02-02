using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExampleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rootCounter;

    private void OnEnable() {
        GameHandler.OnRootPickup += updateCounter;
    }

    private void OnDisable() {
        GameHandler.OnRootPickup -= updateCounter;
    }

    private void updateCounter () {
        rootCounter.text = "Roots: " + GameHandler.NumberOfRoots;
    }
}
