using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestRoot : MonoBehaviour
{
    bool isFocused;

    Vector3 originalScale;
    Vector3 focusedScale;

    private void OnEnable() {
        originalScale = transform.localScale;
        focusedScale = originalScale * 1.15f;
    }

    private void OnMouseEnter() {
        isFocused = true;
        transform.localScale = focusedScale;
    }

    private void OnMouseExit() {
        isFocused = false;
        transform.localScale = originalScale;
    }
}
