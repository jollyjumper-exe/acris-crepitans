using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool isTapped;
    public void OnTap()
    {
        isTapped = true;
    }

    private void Update()
    {
        isTapped = false;
    }
}