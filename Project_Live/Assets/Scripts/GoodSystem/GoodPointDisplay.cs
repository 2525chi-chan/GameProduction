using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�쐬�ҁF�K��

public class GoodPointDisplay : MonoBehaviour
{
    [Header("�����˃A�N�V����1�Ɏg�p����|�C���g�~�ϗʕ\���p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI goodPointText1;
    [Header("�����˃A�N�V����2�Ɏg�p����|�C���g�~�ϗʕ\���p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI goodPointText2;
    [Header("�����˃A�N�V����3�Ɏg�p����|�C���g�~�ϗʕ\���p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI goodPointText3;
    [Header("�����˃A�N�V����4�Ɏg�p����|�C���g�~�ϗʕ\���p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI goodPointText4;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] GoodAction goodAction;

    void Update()
    {
        //���݂̊e�����˃A�N�V�����p�̃|�C���g�~�ϗʂ���ʂɕ\��
        goodPointText1.text = "GP1:" + goodAction.CurrentGoodPoint1.ToString();
        goodPointText2.text = "GP2:" + goodAction.CurrentGoodPoint2.ToString();
        goodPointText3.text = "GP3:" + goodAction.CurrentGoodPoint3.ToString();
        goodPointText4.text = "GP4:" + goodAction.CurrentGoodPoint4.ToString();
    }
}
