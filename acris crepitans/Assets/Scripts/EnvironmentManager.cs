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

    private float totalScrollDistance = 0f;

    private void Start()
    {
        mainCamera = Camera.main;

        for (int i = 0; i < tileCount; i++)
        {
            Vector3 spawnPos = new Vector3(0, i * tileHeight, 1);
            GameObject tile = CreateTileWithObstacles(spawnPos);
            activeTiles.Add(tile);
        }
    }

    private void Update()
    {
        float deltaY = scrollSpeed * Time.deltaTime;
        totalScrollDistance += deltaY;
        GameManager.Instance.UpdateCrawledHeight(totalScrollDistance);

        scrollSpeed = GameManager.Instance.GetCurrentSpeed();

        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            GameObject tile = activeTiles[i];

            tile.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

            if (IsBelowScreen(tile))
            {
                Destroy(tile);
                activeTiles.RemoveAt(i);

                float highestY = GetHighestTileY();
                Vector3 spawnPos = new Vector3(0, highestY + tileHeight, 1);
                GameObject newTile = CreateTileWithObstacles(spawnPos);
                activeTiles.Add(newTile);
            }
        }
    }

    private GameObject CreateTileWithObstacles(Vector3 position)
    {
        int currentIntensity = GameManager.Instance.GetCurrentObstacleIntensity();

        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, tileParent);
        TileManager tileManager = tile.GetComponent<TileManager>(); 
        tileManager.SpawnObstacles(tile, currentIntensity);
        return tile;
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
