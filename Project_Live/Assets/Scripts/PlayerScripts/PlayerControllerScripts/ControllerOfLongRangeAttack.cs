using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//�쐬�ҁF�K��

public class ControllerOfLongRangeAttack : MonoBehaviour
{
    [SerializeField] LongRangeAttack longRangeAttack;
    [SerializeField] InputAction cancelAction; //�A�N�V�����𖳌����������

    private void OnEnable()
    {
        cancelAction.Enable();
    }

    private void OnDisable()
    {
        cancelAction.Disable();
    }

    public void CallLongRangeAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || cancelAction.IsPressed()) return;

        longRangeAttack.ShotBullet();
    }
}
