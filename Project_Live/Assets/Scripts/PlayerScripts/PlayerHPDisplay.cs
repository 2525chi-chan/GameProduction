using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�쐬�ҁF�K��

public class PlayerHPDisplay : MonoBehaviour
{
    [Header("HP�̕\���Ɏg�p����e�L�X�g")]
    [SerializeField] TextMeshProUGUI hpText;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] PlayerStatus playerStatus;

    void Update()
    {
        hpText.text = "HP" + playerStatus.Hp;
        //hpText.text.ToString();
    }
}
