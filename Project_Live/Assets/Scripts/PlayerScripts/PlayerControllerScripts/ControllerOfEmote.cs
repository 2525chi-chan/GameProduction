using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者：桑原

public class ControllerOfEmote : MonoBehaviour
{
    public void CallEmote1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.Emote1Event(); //エモート1の呼び出し
    }

    public void CallEmote2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.Emote2Event(); //エモート2の呼び出し
    }

    public void CallEmote3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.Emote3Event(); //エモート3の呼び出し
    }

    public void CallEmote4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.Emote4Event(); //エモート4の呼び出し
    }
}
