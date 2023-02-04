using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DirtTile : MonoBehaviour
{
    private static readonly float TIME_TO_GROW = 15;

    public enum TileState {
        Empty,
        Growing,
        Grown
    }

    public TileState tileState = TileState.Empty;

    [SerializeField] private RootRenderer[] roots;

    private bool isFocused = false;

    private float originalScale;
    private float focusedScale;
    Coroutine growCoroutine;

    private void Awake() {
        originalScale = transform.localScale.x;
        focusedScale = originalScale * 1.075f;
        ResetRootScale();
    }

    private void Update() {
        if (isFocused && Input.GetMouseButtonDown(0) && tileState == TileState.Empty) {
            PlantRoots(RootAttributes.Default());
        }

        if (isFocused && Input.GetMouseButtonDown(1) && tileState == TileState.Grown) {
            HarvestRoots();
        }
    }

    private void OnMouseEnter() {
        isFocused = true;
        transform.localScale = new Vector3(focusedScale, transform.localScale.y, focusedScale);
    }

    private void OnMouseExit() {
        isFocused = false;
        transform.localScale = new Vector3(originalScale, transform.localScale.y, originalScale);
    }

    public void PlantRoots (RootAttributes attributes) {
        foreach (RootRenderer root in roots)
        {
            root.transform.rotation = Quaternion.Euler(0, 360 * UnityEngine.Random.value, 0);
            root.Inititialise(RootAttributes.MakeMutatedCopy(attributes));
        }

        tileState = TileState.Growing;
        growCoroutine = StartCoroutine(AnimateGrowth());
    }

    public void HarvestRoots () {
        HarvestUI.HarvestRoots(roots.ToList());

        tileState = TileState.Empty;
        ResetRootScale();
    }

    private void ResetRootScale()
    {
        foreach (var root in roots)
        {
            root.transform.localScale = Vector3.zero;
        }
    }

    IEnumerator AnimateGrowth () {
        float progress = 0f;
        while (progress <= TIME_TO_GROW)
        {
            foreach (var root in roots)
            {
                root.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress / TIME_TO_GROW);
            }
            progress += Time.deltaTime;
            yield return null;
        }

        tileState = TileState.Grown;
    }
}
