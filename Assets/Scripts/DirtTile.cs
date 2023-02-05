using System.Collections;
using System.Linq;
using UnityEngine;

public class DirtTile : MonoBehaviour
{
    private static readonly float TIME_TO_GROW = 15;

    public enum TileState
    {
        ForSale,
        Empty,
        Growing,
        Grown
    }

    public TileState tileState = TileState.Empty;
    [SerializeField] private int price;
    [SerializeField] private RootRenderer[] roots;

    private bool isFocused = false;

    private float originalScale;
    private float focusedScale;
    private float grownScale;
    Coroutine growCoroutine;

    float maxTimer = 0.8f;
    public float timer = 0;
    bool pulseIncreasing = true;

    private RootAttributes rootAttributes;

    private void Awake() {
        originalScale = transform.localScale.x;
        focusedScale = originalScale * 1.075f;
        grownScale = originalScale * 1.055f;
        ResetRootScale();
    }

    private void Update() {
        if (CanvasManager.IsHarvestActive) return;

        if (tileState == TileState.Grown && !isFocused) {
            if (pulseIncreasing) {
                timer += Time.deltaTime;
                if (timer > maxTimer) pulseIncreasing = false;
            }
            else {
                timer -= Time.deltaTime;
                if (timer < 0) pulseIncreasing = true;
            }

            Vector3 newScale = Vector3.Lerp(new Vector3(originalScale, transform.localScale.y, originalScale), new Vector3(grownScale, transform.localScale.y, grownScale), timer / maxTimer);
            transform.localScale = newScale;
        }
    }

    private void OnMouseDown() {
        if (tileState == TileState.ForSale && PlayerStats.money >= price)
        {
            tileState = TileState.Empty;
            PlayerStats.money -= price;
            Debug.Log("Money: " + PlayerStats.money);
        }
        else if (tileState == TileState.Empty)
        {
            PlantRoots(PlayerStats.GetInventoryItem(GameHandler.CurrentSelection));
        }
        else if (tileState == TileState.Grown)
        {
            HarvestRoots();
        }
    }

    private void OnMouseEnter() {
        if (!canHover()) return;
        isFocused = true;
        transform.localScale = new Vector3(focusedScale, transform.localScale.y, focusedScale);
    }

    private void OnMouseExit() {
        if (!canHover()) return;
        isFocused = false;
        transform.localScale = new Vector3(originalScale, transform.localScale.y, originalScale);
    }

    private bool canHover () {
        if (CanvasManager.IsHarvestActive) return false;
        if (tileState == TileState.Growing) return false;
        if (tileState == TileState.Empty && GameHandler.CurrentSelection == -1) return false;

        return true;
    }

    public void PlantRoots (RootAttributes attributes) {
        if (attributes == null) return;

        rootAttributes = attributes;

        roots[0].Inititialise(attributes);

        for (int i = 1; i < roots.Length; i++)
        {
            RootRenderer root = roots[i];
            root.transform.rotation = Quaternion.Euler(0, 360 * UnityEngine.Random.value, 0);
            root.Inititialise(RootAttributes.MakeMutatedCopy(attributes));
        }

        PlayerStats.RemoveFromInventory(attributes);

        tileState = TileState.Growing;
        growCoroutine = StartCoroutine(AnimateGrowth());

        transform.localScale = new Vector3(originalScale, transform.localScale.y, originalScale);
        isFocused = false;
    }

    public void HarvestRoots () {
        CanvasManager.HarvestRoots(rootAttributes, roots.Select(rr => rr.GetAttributes()).ToList());

        tileState = TileState.Empty;
        ResetRootScale();

        transform.localScale = new Vector3(originalScale, transform.localScale.y, originalScale);
        isFocused = false;
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
