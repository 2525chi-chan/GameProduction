//using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFallAttack_Boss : MonoBehaviour
{
    EnemyActionStateMachine stateMachine;
    public enum AttackState { Move, Idle, ShowAttackArea, Attack, Cooldown, Exit }

    [Header("隕石として落下させるプレハブ")]
    [SerializeField] GameObject meteorPrefab;
    [Header("落石範囲（コライダー）")]
    [SerializeField] BoxCollider fallArea;
    [Header("隕石生成時の初期高度")]
    [SerializeField] float spawnHeight = 30f;

    [Header("攻撃待機中に出力する音")]
    [SerializeField] AudioClip chargeSound;
    [Header("攻撃生成時に出力する音")]
    [SerializeField] AudioClip attackSound;
    [Header("隕石落下時に出力する音")]
    [SerializeField] AudioClip fallSound;

    [Header("隕石数")]
    [SerializeField] int meteorCount = 5;
    [Header("隕石同士の距離")]
    [SerializeField] float minDistanceMeteors = 3f;
    [Header("地面に着弾するまでの時間")]
    [SerializeField] float timeToGround = 5f;
    [Header("1つ目の隕石落下後、次の隕石が落下するまでの間隔")]
    [SerializeField] float meteorInterval = 0.5f;

    [Header("照準をプレイヤーに合わせる時間")]
    [SerializeField] float aimRotationDuration = 2f;
    [Header("攻撃後のクールタイム")]
    [SerializeField] float attackCoolTime = 1f;
    [Header("ボスの位置")]
    [SerializeField] Transform bossPos;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] Animator animator;
    [SerializeField] MultiAttackWarningController multiWarningController;

    AttackState currentAttackState = AttackState.Move;
    AttackState previousAttackState;
    float currentTimer = 0f;
    bool hasArrived = false;
    bool isActive = false;
    float arriveThreshold = 0.5f;
    float groundY = 0f;
    Coroutine meteorCoroutine;
    string actionPointTag = "ActionPoint_EnemyBoss";
    Transform targetPosition;
    Transform playerPosition;
    List<Vector3> meteorPositions = new List<Vector3>();
    List<GameObject> warningInstances = new List<GameObject>();
    List<GameObject> meteors;
    AudioSource SE;

     public bool IsActive { get { return isActive; } set { isActive = value; } }
    public bool IsAttacked { get; set; }
    public AttackState CurrentAttackState { get { return currentAttackState; } set { currentAttackState = value; } }

    public void SetStartState()
    {
        currentAttackState = AttackState.Move;
        previousAttackState = currentAttackState;
        hasArrived = false;
        targetPosition = null;
    }

    void Start()
    {
        SE = GameObject.FindWithTag("SE").GetComponent<AudioSource>();
    }

    public void StateProcess()
    {
        if (stateMachine == null)
            stateMachine = GetComponentInParent<EnemyActionStateMachine>();

        if (status.IsDead || !isActive) return;

        switch (currentAttackState)
        {
            case AttackState.Move:
                if (targetPosition == null) SetTargetPosition();

                if (!hasArrived)
                {
                    Vector3 selfXZ = new Vector3(bossPos.position.x, 0f, bossPos.position.z);
                    Vector3 targetXZ = new Vector3(targetPosition.position.x, 0f, targetPosition.position.z);
                    float xzDistance = Vector3.Distance(selfXZ, targetXZ);

                    if (xzDistance <= arriveThreshold)
                    {
                        hasArrived = true;
                        currentTimer = 0f;
                    }

                    else
                    {
                        mover.MoveTowardsPlayer(targetPosition);
                        mover.LookPlayer(targetPosition);
                    }
                }

                if (hasArrived) currentAttackState = AttackState.Idle;
                break;

            case AttackState.Idle:
                hasArrived = false;
                currentTimer += Time.deltaTime;
                mover.LookPlayer(playerPosition);

                if (currentTimer >= aimRotationDuration)
                {
                    meteorPositions.Clear();
                    currentTimer = 0f;
                    currentAttackState = AttackState.ShowAttackArea;
                }
                break;

            case AttackState.ShowAttackArea:
                meteorPositions.Clear();
                List<Vector3> candidates = new List<Vector3>();
                int maxAttempts = 50 * meteorCount;
                int attempt = 0;

                while (meteorPositions.Count < meteorCount && attempt < maxAttempts)
                {
                    Vector3 pos = GetRandomGroundPosition();
                    bool overlaps = false;

                    foreach (Vector3 existing in meteorPositions)
                    {
                        if (Vector3.Distance(pos, existing) < minDistanceMeteors)
                        {
                            overlaps = true;
                            break;
                        }
                    }

                    if (!overlaps)
                        meteorPositions.Add(pos);

                    attempt++;
                }

                warningInstances = multiWarningController.CreateMultiWarnings(meteorPositions);
                PreGenerateMeteors();
                                
                currentAttackState = AttackState.Attack;
                currentTimer = 0f;                 
                break;

            case AttackState.Attack:
                break;

            case AttackState.Cooldown:
                currentTimer += Time.deltaTime;

                if (currentTimer >= attackCoolTime)
                {
                    IsAttacked = false;
                    currentTimer = 0f;
                    stateMachine?.actionEvents?.BossAttackFinishEvent();
                    currentAttackState = AttackState.Idle;
                }
                break;

            case AttackState.Exit: //別の攻撃方法に移行時に呼ぶ処理
                currentTimer = 0f;
                stateMachine?.actionEvents?.BossAttackStartEvent();
                isActive = false;
                IsAttacked = false;
                break;
        }

        if (previousAttackState != currentAttackState)
            previousAttackState = currentAttackState;
    }

    public void PlayChargeSound()
    {
        if (SE != null && chargeSound != null) SE.PlayOneShot(chargeSound);
    }

    public void PlayAttackSound()
    {
        if (SE != null && attackSound != null)
        {
            SE.Stop();
            SE.PlayOneShot(attackSound);
        }
    }

    void PreGenerateMeteors() //隕石の生成処理
    {
        meteors = new List<GameObject>();

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 groundPosXZ = meteorPositions[i];
            Vector3 spawnPos = new Vector3(groundPosXZ.x, spawnHeight, groundPosXZ.z);
            GameObject meteorObj = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
            meteors.Add(meteorObj);
            meteorObj.SetActive(false);

            var hitbox = meteorObj.GetComponent<HitboxTrigger>();
            if (hitbox != null) hitbox.SetOwnerMoveType(EnemyMover.EnemyMoveType.PlayerChase);

            var meteor = meteorObj.GetComponent<Meteor>();
            if (meteor != null && i < warningInstances.Count)
                meteor.SetLinkedWarning(warningInstances[i]);
        }
    }

    public void StartFirstMeteorFall() //1つ目の隕石を落下後、続けて隕石を落としていく処理
    {
        if (meteors == null || meteors.Count == 0) return;

        GameObject firstMeteor = meteors[0];
        firstMeteor.SetActive(true);

        Rigidbody rb = firstMeteor.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            float fallVelocity = (spawnHeight - groundY) / timeToGround;
            rb.linearVelocity = new Vector3(0, -fallVelocity, 0);
            SE.PlayOneShot(fallSound);
        }

        var m = firstMeteor.GetComponent<Meteor>();
        if (m != null) m.DestroyLinkedWarning(); //生成された隕石プレハブと対応する予告プレハブを結び付ける
        else Destroy(firstMeteor);

        StartCoroutine(SequentialMeteorFall(1)); //2個目以降の隕石の落下
    }

    IEnumerator SequentialMeteorFall(int startIndex) //2個目以降の隕石の落下処理
    {
        for (int i = startIndex; i < meteorCount; i++)
        {
            yield return new WaitForSeconds(meteorInterval);
            if (meteors[i] != null)
                StartCoroutine(FallMeteor(meteors[i]));
        }
    }

    IEnumerator FallMeteor(GameObject meteor) //生成された隕石の有効化、地面に落下させる処理
    {
        // 出現
        meteor.SetActive(true);

        Rigidbody rb = meteor.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            float fallVelocity = (spawnHeight - groundY) / timeToGround;
            rb.linearVelocity = new Vector3(0, -fallVelocity, 0);
            SE.PlayOneShot(fallSound);
        }

        while (meteor != null && meteor.transform.position.y > groundY + 0.5f)
            yield return null;

        var m = meteor.GetComponent<Meteor>();
        if (m != null) m.DestroyLinkedWarning(); //生成された隕石プレハブと対応する予告プレハブを結び付ける
        else Destroy(meteor);
    }

    Vector3 GetRandomGroundPosition() //ランダムな位置の取得（y軸における高さは一定）
    {
        if (fallArea == null) return Vector3.zero;
        Bounds bounds = fallArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, groundY, z);
    }

    public void SetTargetPosition()
    {
        GameObject[] actionPoints = GameObject.FindGameObjectsWithTag(actionPointTag);

        if (playerPosition == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("LookPoint");
            playerPosition = player.transform;
        }

        if (actionPoints.Length == 0)
        {
            Debug.LogWarning("ActionPoint_EnemyBossがみつかりません");
            return;
        }

        targetPosition = GetNearestTarget(actionPoints);
    }

    Transform GetNearestTarget(GameObject[] points)
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject point in points)
        {
            float dist = Vector3.Distance(
                new Vector3(transform.position.x, 0f, transform.position.z),
                new Vector3(point.transform.position.x, 0f, point.transform.position.z)
                );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = point.transform;
            }
        }

        return nearest;
    }
}
