using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class DamageToTarget : MonoBehaviour
{
    GameObject hitEffect;

    float damage;
    float forwardKnockbackForce;
    float upwardKnockbackForce;
    float downwardKnockbackForce;
    

    public bool isRagdoll = false;
    public GameObject HitEffect { get { return hitEffect; } set { hitEffect = value; } }
    
    public float Damage { get { return damage; } set { damage = value; } }
    public float ForwardKnockbackForce { get { return forwardKnockbackForce; } set { forwardKnockbackForce = value; } }
    public float UpwardKnockbackForce { get { return upwardKnockbackForce; } set { upwardKnockbackForce = value; } }
    public float DownwardKnockbackForce { get { return downwardKnockbackForce; } set { downwardKnockbackForce = value; } }

    public void AddDamageToPlayer(Collider player) //ダメージを与える
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        // なければ子オブジェクトから探す
        if (playerStatus == null)  playerStatus = player.GetComponentInChildren<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogWarning($"PlayerStatus が {player.name} および、その子に見つかりませんでした");
            return;
        }
        //

        //プレイヤーが回避中はダメージを与えないようにする
        if (playerStatus.CurrentState == PlayerStatus.PlayerState.Invincible)
        {
            //Debug.Log("プレイヤーは攻撃を回避した");
            return;
        }

        playerStatus.Hp -= damage;
        Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null) Instantiate(hitEffect, player.bounds.center, player.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
    }

    public void AddDamageToEnemy(Collider enemy)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

        // なければ子孫オブジェクトから探す
        if (enemyStatus == null) enemyStatus = enemy.GetComponentInChildren<EnemyStatus>();

        if (enemyStatus == null)
        {
            Debug.LogWarning($"EnemyStatus が {enemy.name} および、その子に見つかりませんでした");
            return;
        }
        //

        enemyStatus.Hp -= damage;
        Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null) Instantiate(hitEffect, enemy.bounds.center, enemy.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
    }

    public void AddDamageToObject(Collider obj) //ダメージを与える
    {
        ObjectStatus objStatus = obj.GetComponent<ObjectStatus>();

        // なければ子オブジェクトから探す
        if (objStatus == null) objStatus = obj.GetComponentInChildren<ObjectStatus>();

        if (objStatus == null)
        {
            Debug.LogWarning($"ObjectStatus が {obj.name} および、その子に見つかりませんでした");
            return;
        }
        //

        objStatus.Hp -= damage;
        Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null) Instantiate(hitEffect, obj.bounds.center, obj.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
    }

    public void ApplyKnockback(Collider target) //吹き飛ぶ力を加える
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 forwardDirection = (target.transform.position - transform.position).normalized;

            Vector3 knockback =
                forwardDirection * forwardKnockbackForce
                + Vector3.up * upwardKnockbackForce
                - Vector3.up * downwardKnockbackForce;
            if (isRagdoll)
            {

                EnemyRagdoll enemyRagdoll = target.GetComponent<EnemyRagdoll>();
                if (enemyRagdoll != null)
                {
                    enemyRagdoll.SwitchRagdoll(true);
                }
            }
            rb.AddForce(knockback, ForceMode.Impulse);

        }
    }
}
