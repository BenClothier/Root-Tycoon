using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtTile : MonoBehaviour
{
    private static readonly float TIME_TO_GROW = 15;

    public enum TileState {
        Empty,
        Growing,
        Grown
    }
    [SerializeField] private TileState tileState = TileState.Empty;  
    [SerializeField] private RootRenderer root;
    [SerializeField] private Transform[] sowPositions;

    private bool isFocused = false;

    private float originalScale;
    private float focusedScale;
    Coroutine growCoroutine;

    private List<RootRenderer> rootObjects = new();

    private void Awake() {
        originalScale = transform.localScale.x;
        focusedScale = originalScale * 1.15f;
    }

    private void Update() {
        if (isFocused && Input.GetMouseButtonDown(0) && tileState == TileState.Empty) {
            CreateRoot(RootAttributes.Default());
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

    private void CreateRoot (RootAttributes attributes) {
        foreach (Transform sowPosition in sowPositions){
            var rootObject = Instantiate(root, sowPosition.position, Quaternion.Euler(0, 360 * UnityEngine.Random.value, 0), transform);
            rootObject.Inititialise(RootAttributes.MakeMutatedCopy(attributes));
            rootObjects.Add(rootObject);
        }

        tileState = TileState.Growing;

        growCoroutine = StartCoroutine(growRoot());
    }

    private void PickupRoot () {
        foreach (var rootObject in rootObjects){
            Destroy(rootObject);
        }
        rootObjects.Clear();
        tileState = TileState.Empty;

        GameHandler.PickupRoot();
    }

    IEnumerator growRoot () {
        float progress = 0f;
        while (progress <= TIME_TO_GROW) {
            foreach (var rootObject in rootObjects){
                rootObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress / TIME_TO_GROW);
            }
            progress += Time.deltaTime;
            yield return null;
        }

        tileState = TileState.Grown;
    }
}
