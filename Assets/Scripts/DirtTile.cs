using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtTile : MonoBehaviour
{
    public enum TileState {
        Empty,
        Growing,
        Grown
    }
    [SerializeField] private TileState tileState = TileState.Empty;
    [SerializeField] private Root root;

    private bool isFocused = false;

    private float originalScale;
    private float focusedScale;

    GameObject rootObject;
    Coroutine growCoroutine;

    private void Awake() {
        originalScale = transform.localScale.x;
        focusedScale = originalScale * 1.15f;
    }

    private void Update() {
        if (isFocused && Input.GetMouseButtonDown(0) && tileState == TileState.Empty) {
            CreateRoot();
        }

        if (isFocused && Input.GetMouseButtonDown(1) && tileState == TileState.Grown) {
            PickupRoot();
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

    private void CreateRoot () {
        rootObject = Instantiate(root.Model, transform);
        rootObject.transform.localPosition = new Vector3(0, 0.75f, 0);

        tileState = TileState.Growing;

        growCoroutine = StartCoroutine("growRoot");
    }

    private void PickupRoot () {
        Destroy(rootObject);
        tileState = TileState.Empty;
    }

    IEnumerator growRoot () {
        float progress = 0f;
        while (progress <= root.TimeToGrow) {
            rootObject.transform.localScale = Vector3.Lerp(root.StartScale, root.EndScale, progress / root.TimeToGrow);
            progress += Time.deltaTime;
            yield return null;
        }

        tileState = TileState.Grown;
    }
}
