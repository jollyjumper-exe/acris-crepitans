using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private BackgroundManager backgroundManager;

    private void Update()
    {
        if (inputManager.isTapped)
        {
            Jump();

            backgroundManager.ChangeBackgroundColor();
        }
    }

    private void Jump()
    {
        Debug.Log("Player jumped!");
    }
}
