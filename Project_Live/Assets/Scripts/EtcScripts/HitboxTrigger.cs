using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class HitboxTrigger : MonoBehaviour
{
    [Header("命中を判定する回数")]
    [SerializeField] int MaxHitCount = 1;
    [Header("命中判定を行う時間の間隔")]
    [SerializeField] float hitIntervalTime = 0.5f;

    [Header("必要なコンポーネント")]
    [SerializeField] DamageToTarget damageToTarget;

    Dictionary<Collider, float> hitIntervalTimers = new Dictionary<Collider, float>(); //敵ごとの最後に攻撃が当たってからの経過時間
    Dictionary<Collider, int> hitCounts = new Dictionary<Collider, int>(); //敵ごとの今まで攻撃が命中した回数

    List<string> ignoreTags = new List<string>();
    EnemyMover.EnemyMoveType ownermoveType;

    bool isOwnerSet = false; //プレイヤーの攻撃かどうかの判定用

    void Update() //経過時間の更新
    {
        var keys = new List<Collider>(hitIntervalTimers.Keys);

        foreach (var col in keys)
            hitIntervalTimers[col] += Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        SetIgnoreTags();

        if (ignoreTags.Contains(other.tag)) return; //登録されているタグなら何もしない

        if (!hitCounts.ContainsKey(other)) //初めて攻撃が命中した敵なら
        {
            hitCounts[other] = 0; //命中回数のリセット
            hitIntervalTimers[other] = hitIntervalTime; //経過時間のリセット
        }

        if (hitCounts[other] >= MaxHitCount) return; //命中回数を制限する

        if (hitIntervalTimers[other] >= hitIntervalTime) //ヒット可能な時間が経過していた場合
        {
            ApplyDamage(other);
            
            damageToTarget?.ApplyKnockback(other); //吹き飛び処理

            hitIntervalTimers[other] = 0f; //攻撃命中後の経過時間をリセットする
            hitCounts[other]++; //現在のヒット回数を増やす
            //Debug.Log(hitCounts[other]);
        }
    }

    void OnDisable()
    {
        ResetHits();
    }

    public void ResetHits() //命中回数のリセット
    {
        hitCounts.Clear();
        hitIntervalTimers.Clear();
    }

    public void SetOwnerMoveType(EnemyMover.EnemyMoveType moveType) //攻撃を生成した主の移動タイプを渡す
    {
        ownermoveType = moveType;
        isOwnerSet = true;
    }

    void SetIgnoreTags() //命中判定を行う対象の設定
    {
        ignoreTags.Clear();

        //敵の移動タイプが渡されていない場合、プレイヤーの攻撃とする
        if (!isOwnerSet)
        {
            ignoreTags.Add("Player");
            return;
        }

        switch (ownermoveType)
        {
            //プレイヤーを追う敵 → 味方（敵同士）は無視する
            case EnemyMover.EnemyMoveType.PlayerChase:
            case EnemyMover.EnemyMoveType.BlockPlayer:                
                ignoreTags.Add("Enemy");
                ignoreTags.Add("Breakable");
                break;

            //ステージ破壊型 → 味方（敵同士）、プレイヤーを無視する
            case EnemyMover.EnemyMoveType.StageDestroy:
                ignoreTags.Add("Player");
                ignoreTags.Add("Enemy");
                break;

            default: break;
        }

        //foreach (var tag in ignoreTags)
        //    Debug.Log(tag);
    }

    void ApplyDamage(Collider other) //ダメージ処理
    {
        if (other.CompareTag("Player")) damageToTarget?.AddDamageToPlayer(other); //プレイヤーへのダメージ処理
        else if (other.CompareTag("Enemy")) damageToTarget?.AddDamageToEnemy(other); //敵へのダメージ処理
        else if (other.CompareTag("Breakable")) damageToTarget?.AddDamageToObject(other); //破壊可能オブジェクトへのダメージ処理（仮）
    }
}
