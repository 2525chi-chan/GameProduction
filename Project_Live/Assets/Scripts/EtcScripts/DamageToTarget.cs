using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

//作成者：桑原

public class DamageToTarget : MonoBehaviour
{
    //    [Header("攻撃対象を当たり判定の中央に引き寄せるか")]
    //    [SerializeField] bool enableSuctionMode = false;
    //    [Header("攻撃対象を引き寄せる力")]
    //    [SerializeField] float suctionForce = 10f;

    [Header("必要なコンポーネント")]
    [SerializeField] AudioSource SE;

    GameObject hitEffect;
    AudioClip hitSound;
    Transform effectSpawnPoint;

    float damage;
    float forwardKnockbackForce;
    float upwardKnockbackForce;
    float downwardKnockbackForce;
    bool enableForward;
    bool enableSuction;
    float suctionForce;

    public bool isRagdoll = false;
    public GameObject HitEffect { get { return hitEffect; } set { hitEffect = value; } }

    public AudioClip HitSound { get { return hitSound; } set { hitSound = value; } }

    public float Damage { get { return damage; } set { damage = value; } }
    public float ForwardKnockbackForce { get { return forwardKnockbackForce; } set { forwardKnockbackForce = value; } }
    public float UpwardKnockbackForce { get { return upwardKnockbackForce; } set { upwardKnockbackForce = value; } }
    public float DownwardKnockbackForce { get { return downwardKnockbackForce; } set { downwardKnockbackForce = value; } }
    public bool EnableForward { get { return enableForward; } set { enableForward = value; } }
    public bool EnableSuction { get { return enableSuction; } set { enableSuction = value; } }
    public float SuctionForce { get { return suctionForce; } set { suctionForce = value; } }

    public void AddDamageToPlayer(Collider player) //ダメージを与える
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        Live2DController controller= player.GetComponentInChildren<Live2DController>();
        Live2DTalkPlayer talkPlayer = controller.GetComponentInChildren<Live2DTalkPlayer>();
        // なければ子オブジェクトから探す
        if (playerStatus == null)  playerStatus = player.GetComponentInChildren<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogWarning($"PlayerStatus が {player.name} および、その子に見つかりませんでした");
            return;
        }
        else if(controller == null)
        {
            Debug.LogWarning($"Live2DController が {player.name} および、その子に見つかりませんでした");
            return;
        }
        //

        //プレイヤーが回避中はダメージを与えないようにする
        if (playerStatus.CurrentState == PlayerStatus.PlayerState.Invincible)
        {
            //Debug.Log("プレイヤーは攻撃を回避した");
            return;
        }

      DamageReaction( playerStatus, controller,talkPlayer);

        playerStatus.Hp -= damage;
        //Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null) Instantiate(hitEffect, player.bounds.center, player.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
    }
    public void DamageReaction(PlayerStatus playerStatus, Live2DController controller,Live2DTalkPlayer talkPlayer)
    {
        if (damage < playerStatus.BigDamageThreshold)
        {
            controller.PlayMotion("Damage_Low");
            talkPlayer.PlayTalk("Damage_Low");
        }
        else
        {
            controller.PlayMotion("Damage_High");
            talkPlayer.PlayTalk("Damage_High");

        }
    }

    public void AddDamageToEnemy(Collider enemy, Transform effectSpawnPoint)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
        // なければ子孫オブジェクトから探す
        if (enemyStatus == null) enemyStatus = enemy.GetComponentInChildren<EnemyStatus>();
        if (enemyStatus == null)
        {
            Debug.LogWarning($"EnemyStatus が {enemy.name} および、その子に見つかりませんでした");
            return;
        }

        enemyStatus.Hp -= damage;
        //Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null)
        {
            Vector3 spawnPosition;
            Quaternion spawnRotation;

            if (enemy.gameObject.name.Contains("Enemy_Boss"))
            {
                spawnPosition = effectSpawnPoint.position;
                spawnRotation = effectSpawnPoint.rotation;
            }

            else
            {
                spawnPosition = enemy.bounds.center;
                spawnRotation = enemy.gameObject.transform.rotation;
            }
            
            Instantiate(hitEffect, spawnPosition, spawnRotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
        }

        if (hitSound != null) SE.PlayOneShot(hitSound);

        enemyStatus.CurrentDamageCount++;
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
        //Debug.Log(damage + "ダメージを与えた");

        if (hitEffect != null) Instantiate(hitEffect, obj.bounds.center, obj.gameObject.transform.rotation); //エフェクトが設定されていたら、命中時にエフェクトを生成する
    }

    public void ApplyKnockback(Collider target) //吹き飛ぶ力を加える
    {
        if (enableSuction)
        {
            ApplySuction(target);
            return;
        }

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

    public void ApplyForward(Collider target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 forwardDirection = (target.transform.position - transform.position).normalized;

            Vector3 knockback =
                Vector3.forward * forwardKnockbackForce
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

    void ApplySuction(Collider target) //引き寄せる力を加える
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb == null || rb.isKinematic) return;

        Vector3 center = transform.position;

        Vector3 direction = (center - target.transform.position).normalized;
        direction.y = 0f;

        Vector3 pullForce = direction * suctionForce
                + Vector3.up * upwardKnockbackForce
                - Vector3.up * downwardKnockbackForce;

        if (isRagdoll)
        {
            EnemyRagdoll enemyRagdoll = target.GetComponent<EnemyRagdoll>();
            if (enemyRagdoll != null)
                enemyRagdoll.SwitchRagdoll(true);
        }

        rb.AddForce(pullForce, ForceMode.Impulse);
    }
}
