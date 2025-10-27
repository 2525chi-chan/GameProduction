using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//作成者：桑原大悟

[System.Serializable]
public class GoodActionParameters
{
#pragma warning disable 0414
    [SerializeField] string actionName = "未設定";
#pragma warning restore 0414
    [Header("発動に必要ないいね数")]
    [SerializeField] int goodCost = 100;
    [Header("攻撃が発生するまでの時間")]
    [SerializeField] float actionInterval = 1f;
    [Header("待機状態に移行するまでの時間(現在未使用)")]
    [SerializeField] float changeStateInterval = 2f;
    [Header("いいねアクション使用時に発生するエフェクト")]
    [SerializeField] public GameObject goodActionUsedEffect;
    [Header("アクション発動中、ダメージを無効化するかどうか")]
    [SerializeField] bool isInvincible = false;
    
  
    public int GoodCost { get { return goodCost; } }
    public float ActionInterval { get { return actionInterval;} }
    public float ChangeStateInterval { get { return changeStateInterval;} }
    public GameObject GoodActionUsedEffect { get { return goodActionUsedEffect; } }
    public bool IsInvincible { get { return isInvincible; } } 
   
}

public class GoodAction : MonoBehaviour
{
    [Header("イイネアクション1")]
    [SerializeField] GoodActionParameters goodAction1;
    [Header("イイネアクション2")]
    [SerializeField] GoodActionParameters goodAction2;
    [Header("イイネアクション3")]
    [SerializeField] GoodActionParameters goodAction3;
    [Header("イイネアクション4")]
    [SerializeField] GoodActionParameters goodAction4;

    [Header("必要なコンポーネント")]
    [SerializeField] GoodSystem goodSystem;
    [SerializeField] RushAttack rushAttack;
    [SerializeField] WideRangeAttack wideAttack;
    [SerializeField] LongRangeAttack longRangeAttack;
    [SerializeField] ContinuosHitAttack continuosHitAttack;
    [SerializeField] ThrowBomb throwBomb;
    [SerializeField] ExplosionAttack explosionAttack;

    float currentGoodNum = 0;
    int currentGoodPoint1 = 0;
    int currentGoodPoint2 = 0;
    int currentGoodPoint3 = 0;
    int currentGoodPoint4 = 0;

    public GoodActionParameters GoodAction1Parameters { get { return goodAction1; } }
    public GoodActionParameters GoodAction2Parameters { get { return goodAction2; } }
    public GoodActionParameters GoodAction3Parameters { get { return goodAction3; } }
    public GoodActionParameters GoodAction4Parameters { get { return goodAction4; } }

    public float CurrentGoodNum { get { return currentGoodNum; } }

    public int GoodCost1 { get { return goodAction1.GoodCost; } }
    public int GoodCost2 { get { return goodAction2.GoodCost; } }
    public int GoodCost3 { get { return goodAction3.GoodCost; } }
    public int GoodCost4 {  get { return goodAction4.GoodCost; } }

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

        if (delta <= 0) return; //いいねが増えていない場合は何もしない

        if (currentGoodPoint1 < goodAction1.GoodCost) currentGoodPoint1 += (int)delta;
        if (currentGoodPoint2 < goodAction2.GoodCost) currentGoodPoint2 += (int)delta;
        if (currentGoodPoint3 < goodAction3.GoodCost) currentGoodPoint3 += (int)delta;
        if (currentGoodPoint4 < goodAction4.GoodCost) currentGoodPoint4 += (int)delta;

        //蓄積ポイントが上限を超えないように補正する
        currentGoodPoint1 = Mathf.Min(currentGoodPoint1, goodAction1.GoodCost);
        currentGoodPoint2 = Mathf.Min(currentGoodPoint2, goodAction2.GoodCost);
        currentGoodPoint3 = Mathf.Min(currentGoodPoint3, goodAction3.GoodCost);
        currentGoodPoint4 = Mathf.Min(currentGoodPoint4, goodAction4.GoodCost);

        currentGoodNum = goodSystem.GoodNum;
    }

    public void GoodAction1()
    {
        if (currentGoodPoint1 < goodAction1.GoodCost) return;

        rushAttack.Activate();
        //wideAttack.InstantiateWideRangeAttack();
        //Debug.Log("イイネアクション1発動！");
        currentGoodPoint1 = 0;
    }

    public void GoodAction2()
    {
        if (currentGoodPoint2 < goodAction2.GoodCost) return;

        longRangeAttack.ShotBeam();
        //Debug.Log("イイネアクション2発動！");
        currentGoodPoint2 = 0;
    }

    public void GoodAction3()
    {
        if (currentGoodPoint3 < goodAction3.GoodCost) return;

        throwBomb.Throw();
        //continuosHitAttack.GenerateAttack();
        //Debug.Log("イイネアクション3発動！");
        currentGoodPoint3 = 0;
    }

    public void GoodAction4()
    {
        if (currentGoodPoint4 < goodAction4.GoodCost) return;

        explosionAttack.TriggerExplosions();
        //Debug.Log("イイネアクション4発動！");
        currentGoodPoint4 = 0;
    }
}
