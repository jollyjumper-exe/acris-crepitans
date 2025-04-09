using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

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

}

