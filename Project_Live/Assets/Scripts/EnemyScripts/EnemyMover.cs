using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    enum MoveState { stop, lookOnly, move }

    [Header("移動するか判断するプレイヤーとの距離")]
    [SerializeField] float detectionRange = 10f;
    [Header("停止するか判断するプレイヤーとの距離")]
    [SerializeField] float stopRange = 1f;
    [Header("移動速度")]
    [SerializeField] float moveSpeed = 3f;
    [Header("回転速度")]
    [SerializeField] float rotateSpeed;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus enemyStatus;

    private Transform lookTarget; //追いかける対象

    MoveState moveState;

    void Start()
    {
        moveState = MoveState.stop;
        lookTarget = GameObject.FindGameObjectWithTag("LookPoint").transform;
    }

    void Update()
    {
        if (enemyStatus.IsDead || lookTarget == null) return; //HPが0、または追いかける対象が見つからない場合

        float distance = Vector3.Distance(transform.position, lookTarget.position); //プレイヤーとの距離を算出する

        if (distance <= detectionRange && distance >= stopRange) //一定以上距離が離れたら
            moveState = MoveState.move;

        else if (distance <= detectionRange && distance < stopRange) //一定距離以下まで近づいたら
            moveState = MoveState.lookOnly;

        else moveState = MoveState.stop; //一定以上距離が離れたら

        switch(moveState)
        {
            case MoveState.stop: //停止状態（プレイヤーを追従する必要がない）
                return;

            case MoveState.lookOnly: //プレイヤーの方向を向く処理のみ行う状態
                LookPlayer();
                return;

            case MoveState.move: //プレイヤーの方向を向いて追従する状態
                LookPlayer();
                MoveTowardsPlayer();
                return;

            default:
                return;
        }
    }

    void MoveTowardsPlayer()//プレイヤーに向かって移動する
    {
        Vector3 direction = (lookTarget.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void LookPlayer()//Y軸だけ変える
    {
        Vector3 playerPos = lookTarget.position;
        playerPos.y = lookTarget.transform.position.y;

        Vector3 direction = (playerPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
