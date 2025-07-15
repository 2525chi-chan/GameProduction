using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class HitboxTrigger : MonoBehaviour
{
    [Header("命中判定を行う対象のタグ")]
    [SerializeField] string targetTag = "Enemy";
    [Header("命中を判定する回数")]
    [SerializeField] int MaxHitCount = 1;
    [Header("命中判定を行う時間の間隔")]
    [SerializeField] float hitIntervalTime = 0.5f;
    [Header("命中時に発生するエフェクト")]
    [SerializeField] GameObject hitEffect;

    [Header("必要なコンポーネント")]
    [SerializeField] DamageToTarget damageToTarget;

    //敵ごとの最後に攻撃が当たってからの経過時間
    Dictionary<Collider, float> hitIntervalTimers = new Dictionary<Collider, float>();

    //敵ごとの今まで攻撃が命中した回数
    Dictionary<Collider, int> hitCounts = new Dictionary<Collider, int>();

    void Update() //経過時間の更新
    {
        var keys = new List<Collider>(hitIntervalTimers.Keys);

        foreach (var col in keys)
            hitIntervalTimers[col] += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        if (!hitCounts.ContainsKey(other)) //初めて攻撃が命中した敵なら
        {
            hitCounts[other] = 0; //命中回数のリセット
            hitIntervalTimers[other] = hitIntervalTime; //経過時間のリセット
        }

        if (hitCounts[other] >= MaxHitCount) return; //命中回数を制限する

        if (hitIntervalTimers[other] >= hitIntervalTime) //ヒット可能な時間が経過していた場合
        {
            if (targetTag == "Player") damageToTarget?.AddDamageToPlayer(other.gameObject); //プレイヤーへのダメージ処理
            else if (targetTag == "Enemy") damageToTarget?.AddDamageToEnemy(other.gameObject); //敵へのダメージ処理

            if (hitEffect != null) Instantiate(hitEffect, other.bounds.center, other.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
            
            damageToTarget?.ApplyKnockback(other.gameObject); //吹き飛び処理

            hitIntervalTimers[other] = 0f; //攻撃命中後の経過時間をリセットする
            hitCounts[other]++; //現在のヒット回数を増やす

            //Debug.Log("命中");
        }
    }

    void OnDisable()
    {
        //Debug.Log(currentHitCount + "ヒット");
        ResetHits();
    }

    public void ResetHits() //命中回数のリセット
    {
        hitCounts.Clear();
        hitIntervalTimers.Clear();
    }
}
