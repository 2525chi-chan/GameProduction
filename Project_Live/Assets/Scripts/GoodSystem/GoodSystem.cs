//�쐬�ҁ@����

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodSystem : MonoBehaviour
{
    [Header("�����ː��̃e�L�X�g")]
    [SerializeField] TextMeshProUGUI goodText;
    float goodNum;    //�����ː�
    public float GoodNum { get { return goodNum; } set { goodNum = value; } } //�����ː��̃Q�b�^�[�ƃZ�b�^�[

    // Start is called before the first frame update
    void Start()
    {
        goodNum = 0;    //�J�n���̓[��
    }

    // Update is called once per frame
    void Update()
    {
        goodText.text = goodNum.ToString();     //�����ː���string�ɕϊ����ĉ�ʂɕ\��
        Debug.Log(goodNum);
    }
}
