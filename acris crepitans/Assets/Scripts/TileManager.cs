using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float screenOffset = 0.5f;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int slotsPerWall = 10;

    float coinSpawnChance = 0.05f;

    private void Start()
    {
        float verticalSize = Camera.main.orthographicSize;
        float horizontalSize = verticalSize * Camera.main.aspect;

        float wallThickness = 1f;
        float offset = .3f;

        float leftX = -horizontalSize - (wallThickness / 2f - offset);
        float rightX = horizontalSize + (wallThickness / 2f - offset);

        leftWall.localPosition = new Vector3(leftX, 0f, 0f);
        rightWall.localPosition = new Vector3(rightX, 0f, 0f);
    }

    public void SpawnObstacles(GameObject tile, int intensity)
    {
        HashSet<string> usedSlots = new HashSet<string>();
        HashSet<int> leftIndices = new HashSet<int>();
        HashSet<int> rightIndices = new HashSet<int>();

        int attempts = 0;
        int maxAttempts = intensity * 3;

        while (usedSlots.Count < intensity && attempts < maxAttempts)
        {
            attempts++;

            bool spawnLeft = Random.value < 0.5f;
            Transform wall = spawnLeft ? leftWall : rightWall;
            int slotIndex = Random.Range(0, slotsPerWall);
            string slotName = $"{(spawnLeft ? "L" : "R")}_{slotIndex}";

            if (usedSlots.Contains(slotName)) continue;
            if (IsForbiddenSlot(spawnLeft, slotIndex, leftIndices, rightIndices)) continue;

            Transform slot = wall.Find($"Slot_{slotIndex}");
            if (slot != null)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, slot);
                obstacle.transform.localPosition = Vector3.zero;

                if (!spawnLeft)
                {
                    Vector3 scale = obstacle.transform.localScale;
                    scale.x *= -1;
                    obstacle.transform.localScale = scale;
                }

                usedSlots.Add(slotName);
                if (spawnLeft) leftIndices.Add(slotIndex);
                else rightIndices.Add(slotIndex);
            }
        }

        for (int i = 0; i < slotsPerWall; i++)
        {
            string leftSlotName = $"L_{i}";
            string rightSlotName = $"R_{i}";

            if (!usedSlots.Contains(leftSlotName) && Random.value < coinSpawnChance)
            {
                Transform slot = leftWall.Find($"Slot_{i}");
                if (slot != null)
                {
                    GameObject coin = Instantiate(coinPrefab, slot);
                    coin.transform.localPosition = Vector3.zero;
                }
            }

            if (!usedSlots.Contains(rightSlotName) && Random.value < coinSpawnChance)
            {
                Transform slot = rightWall.Find($"Slot_{i}");
                if (slot != null)
                {
                    GameObject coin = Instantiate(coinPrefab, slot);
                    coin.transform.localPosition = Vector3.zero;

                    Vector3 scale = coin.transform.localScale;
                    scale.x *= -1;
                    coin.transform.localScale = scale;
                }
            }
        }
    }

    private bool IsForbiddenSlot(bool spawnLeft, int slotIndex, HashSet<int> leftIndices, HashSet<int> rightIndices)
    {
        HashSet<int> otherSide = spawnLeft ? rightIndices : leftIndices;

        return otherSide.Contains(slotIndex) 
            || otherSide.Contains(slotIndex - 1) 
            || otherSide.Contains(slotIndex + 1)
            || (spawnLeft && slotIndex == 0 && rightIndices.Contains(1)); 
    }

}
