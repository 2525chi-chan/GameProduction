using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("移動するか判断するプレイヤーとの距離")]
    [SerializeField] float detectionRange = 10f;
    [Header("停止するか判断するプレイヤーとの距離")]
    [SerializeField] float stopRange = 1f;
    [Header("移動速度")]
    [SerializeField] float moveSpeed = 3f;
    [Header("回転速度")]
    [SerializeField] float rotateSpeed;

    private Transform lookTarget; //追いかける対象

    void Start()
    {
        lookTarget = GameObject.FindGameObjectWithTag("LookPoint").transform;
    }

    void Update()
    {
        if (lookTarget == null) return;

        float distance = Vector3.Distance(transform.position, lookTarget.position);

        if (distance <= detectionRange && distance >= stopRange) MoveTowardsPlayer();

    }
    void MoveTowardsPlayer()
    {
        Vector3 direction = (lookTarget.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
        LookPlayer();
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
