using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//�쐬�ҁF�K��

public class ControllerOfCloseAttack : MonoBehaviour
{
    [SerializeField] CloseAttack closeAttack;
    [SerializeField] InputAction cancelAction; //�A�N�V�����𖳌����������

    public event System.Action OnCloseAttackPerformed;

    private void OnEnable()
    {
        cancelAction.Enable();
    }

    private void OnDisable()
    {
        cancelAction.Disable();
    }

    public void CallCloseAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || cancelAction.IsPressed()) return;

        closeAttack.TryAttack(); //�ߐڍU�������̌Ăяo��
        OnCloseAttackPerformed?.Invoke(); //�������Ă΂ꂽ���Ƃ�ʒm����
    }
}
