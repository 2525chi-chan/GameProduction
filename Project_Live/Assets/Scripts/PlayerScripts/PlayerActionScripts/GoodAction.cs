using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�쐬�ҁF�K�����

public class GoodAction : MonoBehaviour
{
    [Header("�C�C�l�A�N�V����1�̍Ďg�p�܂ł̎���")]
    [SerializeField] float intervalA = 3f;
    [Header("�C�C�l�A�N�V����2�̍Ďg�p�܂ł̎���")]
    [SerializeField] float intervalB = 3f;
    [Header("�C�C�l�A�N�V����3�̍Ďg�p�܂ł̎���")]
    [SerializeField] float intervalC = 3f;
    [Header("�C�C�l�A�N�V����4�̍Ďg�p�܂ł̎���")]
    [SerializeField] float intervalD = 3f;

    private float actionTimerA = 0f;
    private float actionTimerB = 0f;
    private float actionTimerC = 0f;
    private float actionTimerD = 0f;

    void Start()
    {
        actionTimerA = intervalA; actionTimerB = intervalB;
        actionTimerC = intervalC; actionTimerD = intervalD;
    }

    void Update()
    {
        if (actionTimerA < intervalA)
            actionTimerA += Time.deltaTime;

        if (actionTimerB < intervalB)
            actionTimerB += Time.deltaTime;

        if (actionTimerC < intervalC)
            actionTimerC += Time.deltaTime;

        if (actionTimerD < intervalD)
            actionTimerD += Time.deltaTime;
    }

    public void GoodAction1()
    {
        if (actionTimerA < intervalA) return;

        Debug.Log("�C�C�l�A�N�V����1�����I");
        actionTimerA = 0f;
    }

    public void GoodAction2()
    {
        if (actionTimerB < intervalB) return;

        Debug.Log("�C�C�l�A�N�V����2�����I");
        actionTimerB = 0f;
    }

    public void GoodAction3()
    {
        if (actionTimerC < intervalC) return;

        Debug.Log("�C�C�l�A�N�V����3�����I");
        actionTimerC = 0f;
    }

    public void GoodAction4()
    {
        if (actionTimerD < intervalD) return;

        Debug.Log("�C�C�l�A�N�V����4�����I");
        actionTimerD = 0f;
    }
}
