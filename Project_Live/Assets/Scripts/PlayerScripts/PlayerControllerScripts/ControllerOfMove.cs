using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//�쐬�F�K��

public class ControllerOfMove : MonoBehaviour
{
    [SerializeField] MovePlayer movePlayer;

    //public event System.Action OnCloseMovePerformed;

    public void GetInputVector(InputAction.CallbackContext context)
    {
        Vector2 getVec = context.ReadValue<Vector2>(); //���͕������擾����
        Vector3 moveVec = new Vector3(getVec.x, 0, getVec.y);
        movePlayer.GetMoveVector(moveVec); //�v���C���[�̈ړ��X�N���v�g�ɒl��n��

        //OnCloseMovePerformed?.Invoke(); //�������Ă΂ꂽ���Ƃ�ʒm����
    }
}
