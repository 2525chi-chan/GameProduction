using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class CameraDirectionCalculator : MonoBehaviour
{
    Vector3 camForward;

    Vector3 camRight;

    public Vector3 CamForWard { get { return camForward; } }

    public Vector3 CamRight { get { return camRight; } }

    void Update()
    {
        GetCameraVectors();
    }

    private void GetCameraVectors() //�J�����̊e�x�N�g���̌v�Z
    {
        camForward = Camera.main.transform.forward; //�J�����̐��ʃx�N�g�����擾
        camRight = Camera.main.transform.right; //�J�����̉E�����x�N�g�����擾

        //�e�x�N�g���𐅕��ʏ�̕����x�N�g���ɂ���
        camForward.y = 0;
        camRight.y = 0;

        //�e�x�N�g���̐��K��
        camForward.Normalize();
        camRight.Normalize();
    }
}
