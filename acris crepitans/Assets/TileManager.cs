using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int slotsPerWall = 10;

    private void Start()
    {
        float verticalSize = Camera.main.orthographicSize;
        float horizontalSize = verticalSize * Camera.main.aspect;

        float wallThickness = 1f;
        float offset = .3f;

        float leftX = -horizontalSize - (wallThickness/2f - offset);
        float rightX = horizontalSize + (wallThickness/2f - offset);

        leftWall.localPosition = new Vector3(leftX, 0f, 0f);
        rightWall.localPosition = new Vector3(rightX, 0f, 0f);
    }

    public void SpawnObstacles(GameObject tile, int intensity)
    {
        Transform leftWall = tile.transform.Find("LeftWall");
        Transform rightWall = tile.transform.Find("RightWall");

        for (int i = 0; i < intensity; i++)
        {
            bool spawnLeft = Random.value < 0.5f;
            Transform wall = spawnLeft ? leftWall : rightWall;

            int slotIndex = Random.Range(0, slotsPerWall);
            Transform slot = wall.Find($"Slot_{slotIndex}");

            if (slot != null)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, slot);
                obstacle.transform.localPosition = Vector3.zero;

                if (!spawnLeft)
                {
                    Vector3 scale = obstacle.transform.localScale;
                    scale.x *= -1; // Flip on X-axis
                    obstacle.transform.localScale = scale;
                }
            }
        }
    }
}

