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

    Vector3 minPulse = new Vector3(1, 1, 1);
    Vector3 maxPulse = new Vector3(1.0625f, 1.0625f, 1.0625f);
    float maxTimer = 1.5f;
    public float timer = 0;
    bool pulseIncreasing = true;

    private void Awake() {
        originalScale = transform.localScale.x;
        focusedScale = originalScale * 1.075f;
        ResetRootScale();
    }

    private void Update() {
        if (CanvasManager.IsHarvestActive) return;

        if (tileState == TileState.Grown) {
            if (pulseIncreasing) {
                timer += Time.deltaTime;
                if (timer > maxTimer) pulseIncreasing = false;
            }
            else {
                timer -= Time.deltaTime;
                if (timer < 0) pulseIncreasing = true;
            }

            Vector3 newScale = Vector3.Lerp(minPulse, maxPulse * 1.5f, timer / maxTimer);

            foreach (RootRenderer rootRenderer in roots) {
                rootRenderer.transform.localScale = newScale;
            }
        }
    }

    private void OnMouseDown() {
        if (tileState == TileState.Empty) {
            PlantRoots(PlayerStats.GetInventoryItem(GameHandler.CurrentSelection));
        }

        if (tileState == TileState.Grown) {
            HarvestRoots();
        }
    }

    private void OnMouseEnter() {
        if (CanvasManager.IsHarvestActive || tileState == TileState.Growing || GameHandler.CurrentSelection == -1) return;
        transform.localScale = new Vector3(focusedScale, transform.localScale.y, focusedScale);
    }

    private void OnMouseExit() {
        if (CanvasManager.IsHarvestActive || tileState == TileState.Growing || GameHandler.CurrentSelection == -1) return;
        transform.localScale = new Vector3(originalScale, transform.localScale.y, originalScale);
    }

    public void PlantRoots (RootAttributes attributes) {
        if (attributes == null) return;

        foreach (RootRenderer root in roots)
        {
            root.transform.rotation = Quaternion.Euler(0, 360 * UnityEngine.Random.value, 0);
            root.Inititialise(RootAttributes.MakeMutatedCopy(attributes));
        }

        PlayerStats.RemoveFromInventory(attributes);

        tileState = TileState.Growing;
        growCoroutine = StartCoroutine(AnimateGrowth());
    }

    public void HarvestRoots () {
        CanvasManager.HarvestRoots(roots.Select(rr => rr.GetAttributes()).ToList());

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
