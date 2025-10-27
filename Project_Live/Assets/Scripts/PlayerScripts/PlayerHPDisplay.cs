using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//作成者：桑原

public class PlayerHPDisplay : MonoBehaviour
{
    [Header("HPの表示に使用する画像")]
    [SerializeField] Image hpImage;

    [Header("必要なコンポーネント")]
    [SerializeField] PlayerStatus playerStatus;

    void Update()
    {
        hpImage.fillAmount = (float)playerStatus.Hp / playerStatus.MaxHp;
        // hpText.text = "HP" + playerStatus.Hp;
        //hpText.text.ToString();
    }
}
