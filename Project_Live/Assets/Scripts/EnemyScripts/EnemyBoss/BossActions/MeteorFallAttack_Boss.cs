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
    [Header("地上高さ（y座標）")]
    [SerializeField] float groundY = 0f;
    [Header("隕石生成時の初期高度")]
    [SerializeField] float spawnHeight = 30f;

    [Header("隕石数")]
    [SerializeField] int meteorCount = 5;
    [Header("隕石同士の距離")]
    [SerializeField] float minDistanceMeteors = 3f;
    [Header("生成から落下開始までの待機時間")]
    [SerializeField] float fallDelay = 1.5f;
    [Header("地面に着弾するまでの時間")]
    [SerializeField] float timeToGround = 5f;
    [Header("1つ目の隕石落下後、次の隕石が落下するまでの間隔")]
    [SerializeField] float meteorInterval = 0.5f;

    [Header("照準をプレイヤーに合わせる時間")]
    [SerializeField] float aimRotationDuration = 2f;
    [Header("攻撃するまでの待ち時間")]
    [SerializeField] float attackDuration = 3f;
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
    Coroutine meteorCoroutine;
    string actionPointTag = "ActionPoint_EnemyBoss";
    Transform targetPosition;
    Transform playerPosition;
    List<Vector3> meteorPositions = new List<Vector3>();
    List<GameObject> warningInstances = new List<GameObject>();

    public bool IsActive { get { return isActive; } set { isActive = value; } }
    public AttackState CurrentAttackState { get { return currentAttackState; } set { currentAttackState = value; } }
    public AttackState PreviousAttackState { get { return previousAttackState; } }

    public void SetStartState()
    {
        currentAttackState = AttackState.Move;
        hasArrived = false;
        targetPosition = null;
    }

    public void StateProcess()
    {
        if (stateMachine == null)
            stateMachine = GetComponentInParent<EnemyActionStateMachine>();

        Debug.Log($"[Meteor StateProcess] Active:{isActive}, Dead:{status.IsDead}, State:{currentAttackState}, targetPos:{targetPosition != null}, playerPos:{playerPosition != null}");

        if (status.IsDead || !isActive) return;

        switch (currentAttackState)
        {
            case AttackState.Move:
                if (targetPosition == null) SetTargetPosition();

                if (!hasArrived)
                {
                    Debug.Log("移動状態");

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
                    {
                        meteorPositions.Add(pos);
                    }

                    attempt++;
                }

                warningInstances = multiWarningController.CreateMultiWarnings(meteorPositions);           
                currentAttackState = AttackState.Attack;
                currentTimer = 0f;
                 
                break;

            case AttackState.Attack:
                currentTimer += Time.deltaTime;
                if (currentTimer >= attackDuration)
                {
                    InstanceMeteorAttack();
                    currentTimer = 0f;
                    currentAttackState = AttackState.Cooldown;
                }
                break;

            case AttackState.Cooldown:
                currentTimer += Time.deltaTime;
                if (currentTimer >= attackCoolTime)
                {
                    currentTimer = 0f;
                    stateMachine?.actionEvents?.BossAttackFinishEvent();
                    currentAttackState = AttackState.Idle;
                }
                break;

            case AttackState.Exit: //別の攻撃方法に移行時に呼ぶ処理
                currentTimer = 0f;
                stateMachine?.actionEvents?.BossAttackStartEvent();
                isActive = false;
                break;
        }

        if (previousAttackState != currentAttackState)
            previousAttackState = currentAttackState;
    }

    public void InstanceMeteorAttack()
    {
        if (meteorCoroutine != null)
            StopCoroutine(meteorCoroutine);
        meteorCoroutine = StartCoroutine(MeteorFallCoroutine());
    }

    IEnumerator MeteorFallCoroutine()
    {
        List<GameObject> meteors = new List<GameObject>();

        for (int i = 0; i < meteorCount; i++)
        {
            Vector3 groundPosXZ = meteorPositions[i];

            Vector3 spawnPos = new Vector3(groundPosXZ.x, spawnHeight, groundPosXZ.z);
            GameObject meteorObj = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
            meteors.Add(meteorObj);
            meteorObj.SetActive(false); // 最初は非表示

            var hitbox = meteorObj.GetComponentInChildren<HitboxTrigger>();
            if (hitbox != null)
                hitbox.SetOwnerMoveType(mover.MoveType);

            var meteor = meteorObj.GetComponent<Meteor>();
            if (meteor != null && i < warningInstances.Count)
                meteor.SetLinkedWarning(warningInstances[i]);
        }

        yield return new WaitForSeconds(fallDelay);

        for (int i = 0; i < meteorCount; i++)
        {
            if (meteors[i] != null)
                StartCoroutine(FallMeteor(meteors[i]));

            if (i < meteorCount - 1)
                yield return new WaitForSeconds(meteorInterval);
        }
    }

    IEnumerator FallMeteor(GameObject meteor)
    {
        // 出現
        meteor.SetActive(true);

        Rigidbody rb = meteor.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            float fallVelocity = (spawnHeight - groundY) / timeToGround;
            rb.linearVelocity = new Vector3(0, -fallVelocity, 0);
        }

        while (meteor != null && meteor.transform.position.y > groundY + 0.5f)
            yield return null;

        var m = meteor.GetComponent<Meteor>();
        if (m != null)
            m.DestroyLinkedWarning();

        else
        {
            Destroy(meteor);
            Debug.Log("meteorコンポーネントが取得できませんでした。");
        }
    }

    Vector3 GetRandomGroundPosition()
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
