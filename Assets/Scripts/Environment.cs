using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Transform environmentContainer;
    [SerializeField] private GameObject plotPrefab;

    private const int PLOT_X = 3;
    private const int PLOT_Z = 6;

    private const float DISTANCE_BETWEEN_PLOTS = 5f;

    [ContextMenu("Reset Environment")]
    public void ResetEnvironment () {
        while (environmentContainer.childCount != 0) {
            foreach (Transform child in environmentContainer) DestroyImmediate(child.gameObject);
        }

        for (int plotX = 0; plotX < PLOT_X; plotX++) {
            for (int plotZ = 0; plotZ < PLOT_Z; plotZ++) {
                float xPos = (plotX - ((PLOT_X - 1) / 2)) * DISTANCE_BETWEEN_PLOTS;
                float zPos = (plotZ - (PLOT_Z / 2) + 0.5f) * DISTANCE_BETWEEN_PLOTS;

                GameObject plotObject = Instantiate(plotPrefab, environmentContainer);
                plotObject.transform.localPosition = new Vector3(xPos, 0, zPos);
            }
        }
    }
}
