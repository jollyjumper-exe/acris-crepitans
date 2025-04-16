using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool isTapped { get; private set; }
    public bool isHolding { get; private set; }

    public void OnTap()
    {
        isTapped = true;
    }

    public void OnHold(InputValue ctx)
    {
        float value = ctx.Get<float>();
        if(value == 1){
            isHolding = true;
            Debug.Log("IS HOLDING");
        }
        else{
            isHolding = false;
            Debug.Log("NOT HOLDING");
        }
    }

    public void OnHoldRelease()  // Call this when the hold is released
    {
        isHolding = false;
    }

    private void Update()
    {
        isTapped = false;
    }
}
