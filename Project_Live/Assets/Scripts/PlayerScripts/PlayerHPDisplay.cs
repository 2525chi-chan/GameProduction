using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//�쐬�ҁF�K��

public class PlayerHPDisplay : MonoBehaviour
{
    [Header("HP�̕\���Ɏg�p����摜")]
    [SerializeField] Image hpImage;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] PlayerStatus playerStatus;

    void Update()
    {
        hpImage.fillAmount = (float)playerStatus.Hp / playerStatus.MaxHp;
        // hpText.text = "HP" + playerStatus.Hp;
        //hpText.text.ToString();
    }
}
