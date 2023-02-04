using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestRoot : MonoBehaviour
{
    bool isFocused;

    Vector3 originalScale = new Vector3(0.75f, 0.75f, 0.75f);
    Vector3 focusedScale;

    [HideInInspector] public RootAttributes rootAttributes;

    public event Action OnClick;

    private void OnEnable() {
        focusedScale = originalScale * 1.15f;

        OnMouseExit();
    }

    private void Update() {
        if (isFocused && Input.GetMouseButtonDown(0)) {
            OnClick?.Invoke();
        }
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
