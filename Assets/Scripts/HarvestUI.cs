using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestUI : MonoBehaviour
{
    public static event Action<Transform[]> OnHarvest;
    public static void HarvestRoots (Transform[] rootTransforms) {
        OnHarvest?.Invoke(rootTransforms);
    }

    private void OnEnable() {
        OnHarvest += moveRoots;
    }

    private void OnDisable() {
        OnHarvest -= moveRoots;
    }

    public void moveRoots (Transform[] rootTransforms) {
        foreach (Transform rootTrans in rootTransforms) {
            Debug.Log(rootTrans);
            rootTrans.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        }
    }
}
