using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int tileCount = 5;
    [SerializeField] private float tileHeight = 1f;
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private Transform tileParent;

    private List<GameObject> activeTiles = new List<GameObject>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // Spawn initial tiles upward
        for (int i = 0; i < tileCount; i++)
        {
            Vector3 spawnPos = Vector3.up * i * tileHeight;
            spawnPos = spawnPos + new Vector3(0,0,1);
            GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, tileParent);
            activeTiles.Add(tile);
        }
    }

    private void Update()
    {
        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            GameObject tile = activeTiles[i];

            // Move tile downward
            tile.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

            // If tile is below the screen, destroy and spawn a new one
            if (IsBelowScreen(tile))
            {
                Destroy(tile);
                activeTiles.RemoveAt(i);

                float highestY = GetHighestTileY();
                Vector3 spawnPos = new Vector3(0, highestY + tileHeight, 1);
                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity, tileParent);
                activeTiles.Add(newTile);
            }
        }
    }

    private bool IsBelowScreen(GameObject tile)
    {
        float camBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        float tileTop = tile.transform.position.y + tileHeight / 2f;

        return tileTop < camBottom;
    }

    private float GetHighestTileY()
    {
        float maxY = float.MinValue;
        foreach (var t in activeTiles)
        {
            float y = t.transform.position.y;
            if (y > maxY)
                maxY = y;
        }
        return maxY;
    }
}
