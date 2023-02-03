using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Transform dirtTilesContainer;
    [SerializeField] private GameObject dirtTilePrefab;

    [SerializeField] private Transform grassContainer;
    [SerializeField] private GameObject[] grassPrefabs;

    private const int TILES_X = 5;
    private const int TILES_Z = 5;

    private void CreateDirtTiles () {
        for (int x = 0; x < TILES_X; x++) {
            for (int z = 0; z < TILES_Z; z++) {
                GameObject dirtTile = Instantiate(dirtTilePrefab, dirtTilesContainer);
                int x_coord = x - ((TILES_X - 1) / 2);
                int z_coord = z - ((TILES_Z - 1) / 2);
                dirtTile.transform.localPosition = new Vector3(x_coord, 0f, z_coord);
            }
        }
    }

    private void DestroyDirtTiles () {
        foreach (Transform child in dirtTilesContainer) {
            DestroyImmediate(child.gameObject);
        }

        if (dirtTilesContainer.childCount != 0) DestroyDirtTiles();
    }

    private void CreateGrass () {
        int count = Random.Range(500, 750);

        for (int i = 0; i < count; i++) {
            float x_coord = Random.Range(-10f, 10f);
            float z_coord = Random.Range(-10f, 10f);
            if (x_coord > -2.5f && x_coord < 2.5f && z_coord > -2.5f && z_coord < 2.5f) continue;

            GameObject grass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)], grassContainer);
            grass.transform.localPosition = new Vector3(x_coord, 0, z_coord);
            grass.transform.localRotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
        }
    }

    private void DestroyGrass () {
        foreach (Transform child in grassContainer) {
            DestroyImmediate(child.gameObject);
        }

        if (grassContainer.childCount != 0) DestroyGrass();
    }

    [ContextMenu("Reset Environment")]
    public void ResetEnvironment () {
        DestroyDirtTiles();
        DestroyGrass();

        CreateDirtTiles();
        CreateGrass();
    }
}
