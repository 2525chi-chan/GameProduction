using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�쐬�ҁF�K�����

public class GoodAction : MonoBehaviour
{
    [Header("�C�C�l�A�N�V����1�����ɕK�v�Ȃ����ː�")]
    [SerializeField] int goodCost1 = 100;
    [Header("�C�C�l�A�N�V����2�����ɕK�v�Ȃ����ː�")]
    [SerializeField] int goodCost2 = 100;
    [Header("�C�C�l�A�N�V����3�����ɕK�v�Ȃ����ː�")]
    [SerializeField] int goodCost3 = 100;
    [Header("�C�C�l�A�N�V����4�����ɕK�v�Ȃ����ː�")]
    [SerializeField] int goodCost4 = 100;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] GoodSystem goodSystem;
    [SerializeField] WideRangeAttack wideAttack;
    [SerializeField] LongRangeAttack longRangeAttack;
    [SerializeField] ContinuosHitAttack continuosHitAttack;
    [SerializeField] ExplosionAttack explosionAttack;

    float currentGoodNum = 0;
    int currentGoodPoint1 = 0;
    int currentGoodPoint2 = 0;
    int currentGoodPoint3 = 0;
    int currentGoodPoint4 = 0;

    public float CurrentGoodNum { get { return currentGoodNum; } }

    public int GoodCost1 { get { return goodCost1; } }
    public int GoodCost2 { get { return goodCost2; } }
    public int GoodCost3 { get { return goodCost3; } }
    public int GoodCost4 {  get { return goodCost4; } }

    public int CurrentGoodPoint1 { get { return currentGoodPoint1; } }
    public int CurrentGoodPoint2 { get {  return currentGoodPoint2; } }
    public int CurrentGoodPoint3 { get {  return currentGoodPoint3; } }
    public int CurrentGoodPoint4 { get {  return currentGoodPoint4; } }

    

    void Start()
    {
        currentGoodNum = goodSystem.GoodNum;
    }

    void Update()
    {
        float delta = goodSystem.GoodNum - currentGoodNum;

        if (delta <= 0) return; //�����˂������Ă��Ȃ��ꍇ�͉������Ȃ�

        if (currentGoodPoint1 < goodCost1) currentGoodPoint1 += (int)delta;

        if (currentGoodPoint2 < goodCost2) currentGoodPoint2 += (int)delta;

        if (currentGoodPoint3 < goodCost3) currentGoodPoint3 += (int)delta;

        if (currentGoodPoint4 < goodCost4) currentGoodPoint4 += (int)delta;

        //�~�σ|�C���g������𒴂��Ȃ��悤�ɕ␳����
        currentGoodPoint1 = Mathf.Min(currentGoodPoint1, goodCost1);
        currentGoodPoint2 = Mathf.Min(currentGoodPoint2, goodCost2);
        currentGoodPoint3 = Mathf.Min(currentGoodPoint3, goodCost3);
        currentGoodPoint4 = Mathf.Min(currentGoodPoint4, goodCost4);

        currentGoodNum = goodSystem.GoodNum;
    }

    public void GoodAction1()
    {
        if (currentGoodPoint1 < goodCost1) return;

        wideAttack.InstantiateWideRangeAttack();
        Debug.Log("�C�C�l�A�N�V����1�����I");
        currentGoodPoint1 = 0;
    }

    public void GoodAction2()
    {
        if (currentGoodPoint2 < goodCost2) return;

        longRangeAttack.ShotBeam();
        Debug.Log("�C�C�l�A�N�V����2�����I");
        currentGoodPoint2 = 0;
    }

    public void GoodAction3()
    {
        if (currentGoodPoint3 < goodCost3) return;

        continuosHitAttack.GenerateAttack();
        Debug.Log("�C�C�l�A�N�V����3�����I");
        currentGoodPoint3 = 0;
    }

    public void GoodAction4()
    {
        if (currentGoodPoint4 < goodCost4) return;

        explosionAttack.TriggerExplosions();
        Debug.Log("�C�C�l�A�N�V����4�����I");
        currentGoodPoint4 = 0;
    }
}
