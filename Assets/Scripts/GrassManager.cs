using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager : MonoBehaviour
{
    [ContextMenu("Randomize Rotation")]
    public void RandomizeRotation () {
        foreach (Transform child in transform) {
            child.localRotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        }
    }

    [ContextMenu("Reset Rotation")]
    public void ResetRotation () {
        foreach (Transform child in transform) {
            child.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
