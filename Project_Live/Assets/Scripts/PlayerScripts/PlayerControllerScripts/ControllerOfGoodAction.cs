using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//作成者：桑原

public class ControllerOfGoodAction : MonoBehaviour
{
    [SerializeField] GoodAction goodAction;
    public void CallGoodAction1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.GoodAction1Event(); //イイネアクション1の呼び出し
    }

    public void CallGoodAction2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.GoodAction2Event(); //イイネアクション2の呼び出し
    }

    public void CallGoodAction3(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.GoodAction3Event(); //イイネアクション3の呼び出し
    }

    public void CallGoodAction4(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PlayerActionEvents.GoodAction4Event(); //イイネアクション4の呼び出し
    }
}
