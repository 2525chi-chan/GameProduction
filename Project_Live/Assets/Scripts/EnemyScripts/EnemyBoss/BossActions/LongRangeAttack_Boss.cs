using UnityEngine;

public class LongRangeAttack_Boss : MonoBehaviour
{
    EnemyActionStateMachine stateMachine;
    public enum AttackState { Move, Idle, ShowAttackArea, Attack, Cooldown }
    
    [Header("攻撃判定を持つオブジェクト")]
    [SerializeField] GameObject attackPrefab;
    [Header("攻撃を生成する位置")]
    [SerializeField] Transform attackPosition;
    [Header("照準をプレイヤーに合わせる時間")]
    [SerializeField] float aimRotationDuration = 2f;
    [Header("攻撃するまでの待ち時間")]
    [SerializeField] float attackDuration = 3f;
    [Header("攻撃判定の持続時間")]
    [SerializeField] float attackEnabledTime = 0.2f;
    [Header("攻撃後のクールタイム")]
    [SerializeField] float attackCoolTime = 1f;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] Animator animator;
    [SerializeField] AttackWarningController attackWarningController;

    AttackState currentAttackState = AttackState.Move;
    AttackState previousAttackState;
    float currentTimer = 0f; //経過時間の測定用
    bool hasArrived = false; //目標地点に到達したか
    bool isActive = false;
    float arriveThreshold = 0.5f; //目標地点に到達したか判定する閾値
    string actionPointTag = "ActionPoint_EnemyBoss"; //移動先となるオブジェクトのタグ
    Transform targetPosition; //移動先の位置
    Transform playerPosition; //プレイヤーの位置

    public bool IsActive { get { return isActive; } set { isActive = value; } }

    public AttackState CurrentAttackState { get { return currentAttackState; } }
    public AttackState PreviousAttackState { get { return previousAttackState; } }

    public void SetStartState()
    {
        currentAttackState = AttackState.Move;
    }

    public void StateProcess()
    {
        if (stateMachine == null)
            stateMachine = GetComponentInParent<EnemyActionStateMachine>();

        if (status.IsDead || !isActive) return;
        
        switch (currentAttackState)    
        {  
            case AttackState.Move: //攻撃開始地点に移動する状態
                mover.MoveTowardsPlayer(targetPosition);
                mover.LookPlayer(targetPosition);
                //Debug.Log("移動状態");

                if (!hasArrived)
                {
                    Vector3 selfXZ = new Vector3(transform.position.x, 0f, transform.position.z);
                    Vector3 targetXZ = new Vector3(targetPosition.position.x, 0f, targetPosition.position.z);
                    float xzDistance = Vector3.Distance(selfXZ, targetXZ);

                    if (xzDistance <= arriveThreshold)
                    {
                        hasArrived = true;
                        currentAttackState = AttackState.Idle;
                        currentTimer = 0f;
                    }
                }
                break;

            case AttackState.Idle: //待機状態
                hasArrived = false;
                currentTimer += Time.deltaTime;
                mover.LookPlayer(playerPosition);
                //Debug.Log("待機状態");

                if (currentTimer >= aimRotationDuration)
                {
                    currentTimer = 0f;
                    currentAttackState = AttackState.ShowAttackArea;
                }
                break;

            case AttackState.ShowAttackArea: //攻撃予告を表示している状態
                if (!attackWarningController.IsWarningActive && !attackWarningController.IsWarningFinished)
                {
                    //Debug.Log("予告状態");
                    attackWarningController.ShowAttackWarning(attackPosition);
                }

                if (attackWarningController.IsWarningFinished)
                {
                    currentAttackState = AttackState.Attack;
                    currentTimer = 0f;
                    attackWarningController.IsWarningFinished = false;
                }
                break;

            case AttackState.Attack: //攻撃状態
                currentTimer += Time.deltaTime;
                //Debug.Log("攻撃状態");
                if (currentTimer >= attackDuration)
                {
                    InstanceAttack();
                    currentTimer = 0f;
                    currentAttackState = AttackState.Cooldown;
                }
                break;

            case AttackState.Cooldown: //クールダウン状態
                currentTimer += Time.deltaTime;
                //Debug.Log("クールダウン状態");
                if (currentTimer >= attackCoolTime)
                {
                    currentTimer = 0f;
                    currentAttackState = AttackState.Idle;
                    stateMachine?.actionEvents?.BossAttackFinishEvent();
                }
                break;
        }

        if (previousAttackState != currentAttackState)
            previousAttackState = currentAttackState;
    }

    public void InstanceAttack() //攻撃の生成
    {
        GameObject attackObj = Instantiate(attackPrefab, attackPosition.position, attackPosition.rotation);
        Destroy(attackObj, attackEnabledTime);

        if (mover != null)
        {
            var hitbox = attackObj.GetComponent<HitboxTrigger>();

            if (hitbox == null) hitbox = attackObj.GetComponentInChildren<HitboxTrigger>();

            if (hitbox != null)
                hitbox.SetOwnerMoveType(EnemyMover.EnemyMoveType.PlayerChase); //攻撃判定に、攻撃者の移動タイプを渡す
        }
    }

    public void SetTargetPosition() //移動先の設定
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

    Transform GetNearestTarget(GameObject[] points) //最も近い移動先の取得
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
