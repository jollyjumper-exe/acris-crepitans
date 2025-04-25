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
        }
        else{
            isHolding = false;
        }
    }

    private void Update()
    {
        isTapped = false;
    }
}
