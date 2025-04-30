using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class MovePlayer : MonoBehaviour
{
    [Header("�������I�u�W�F�N�g")]
    [SerializeField] Transform target;
    [Header("�ړ��X�s�[�h")]
    [SerializeField] float speed = 10f;
    [Header("��]�X�s�[�h")]
    [SerializeField] float rotationSpeed = 10f;

    [Header("�K�v�ȃR���|�[�l���g")]
    
    [SerializeField] CameraDirectionCalculator cameraDirectionCalculator;
    [SerializeField] Dodge dodge;

    Vector3 move; //���͒l�擾�p�ϐ�
    Vector3 prev_Move = Vector3.zero; //�O�t���[���̈ړ��l�ۑ��p�ϐ�

    Vector3 moveDirection; //�ړ������p�̕ϐ�

    public Vector3 Move { get { return move; } }
    public Vector3 Prev_Move { get {  return prev_Move; } }

    void Update()
    {
        if (move.magnitude > 0.1f) //�ړ��̓��͂���������
        {
            CalculateMoveDirection();
            RotateTransform();
            MoveTransform();           
        }

        prev_Move = move; //�ړ��̒l��ۑ�
    }

    void CalculateMoveDirection() //�ړ������̌v�Z
    {
        moveDirection = cameraDirectionCalculator.CamForWard * move.z + cameraDirectionCalculator.CamRight * move.x; //�ړ������̎Z�o
    }

    void MoveTransform() //�ړ�����
    {    
        target.transform.position += moveDirection * (speed + dodge.AddDodgeSpeed()) * Time.deltaTime;
    }

    void RotateTransform() //��]����
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

        //��]���x�ɐ�����������
        target.transform.rotation = Quaternion.RotateTowards(target.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void GetMoveVector(Vector3 getVec)
    {
        move = new Vector3(getVec.x, 0, getVec.z); //���͒l�̎擾
    }
}
