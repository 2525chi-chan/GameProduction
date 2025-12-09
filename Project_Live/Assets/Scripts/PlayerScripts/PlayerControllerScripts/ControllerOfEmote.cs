using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者：桑原

public class ControllerOfEmote : MonoBehaviour
{
    [Header("必要なコンポーネント")]
    [SerializeField]RequestManager requestManager;

    void CheckRequest()
    {
        if(requestManager.requestEmoteIsReceipt&&!requestManager.isIntercepting)
        {
            requestManager.SuccessEmoteRequest();
        }
    }

    public void CallEmote1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        CheckRequest();

        PlayerActionEvents.Emote1Event(); //エモート1の呼び出し
    }

    public void CallEmote2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        CheckRequest();

        PlayerActionEvents.Emote2Event(); //エモート2の呼び出し
    }

    public void CallEmote3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        CheckRequest();

        PlayerActionEvents.Emote3Event(); //エモート3の呼び出し
    }

    public void CallEmote4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        CheckRequest();

        PlayerActionEvents.Emote4Event(); //エモート4の呼び出し
    }
}
