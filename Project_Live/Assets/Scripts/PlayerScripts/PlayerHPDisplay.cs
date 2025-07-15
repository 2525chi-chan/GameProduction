using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//作成者：桑原

public class PlayerHPDisplay : MonoBehaviour
{
    [Header("HPの表示に使用するテキスト")]
    [SerializeField] TextMeshProUGUI hpText;

    [Header("必要なコンポーネント")]
    [SerializeField] PlayerStatus playerStatus;

    void Update()
    {
        hpText.text = "HP" + playerStatus.Hp;
        //hpText.text.ToString();
    }
}
