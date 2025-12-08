using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;  


public  enum EnemyMoveState { stop, lookOnly, move }
public class EnemyMover : MonoBehaviour
{
    public EnemyActionEvents actionEvents = new EnemyActionEvents();
    public enum EnemyMoveType { PlayerChase, BlockPlayer, StageDestroy }


    [Header("移動するか判断するプレイヤーとの距離")]
    [SerializeField] float detectionRange = 10f;
    [Header("停止するか判断するプレイヤーとの距離")]
    [SerializeField] float stopRange = 1f;
    [Header("移動速度")]
    [SerializeField] float moveSpeed = 3f;
    [Header("回転速度")]
    [SerializeField] float rotateSpeed;
    [Header("対象との距離を更新する時間間隔")]
    [SerializeField] float distanceCheckInterval = 0.25f;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus enemyStatus;

    private Transform lookTarget; //追いかける対象
    GameObject[] breakables; //破壊できる対象
    EnemyMoveType moveType;
    EnemyMoveState currentMoveState;
    float moveTypeTimer = 0f; //移動タイプ更新時間の計測用
    bool isActiveMove = true;
    bool isActiveRotate = true;

    public EnemyMoveState MoveState { get { return currentMoveState; } }
    public EnemyMoveType MoveType { get { return moveType; } }   
    
    public bool IsActiveMove { get { return isActiveMove; } set { isActiveMove = value; } }
    public bool IsActiveRotate { get { return isActiveRotate; } set { isActiveRotate = value; } }

    public void SetMoveType(EnemyMoveType moveType)
    {
        this.moveType = moveType;
        InitTarget(); //移動タイプに応じてターゲットを設定する
    }
    public void MoveSetState(EnemyMoveState state)
    {
        currentMoveState = state;
    }
    void Start()
    {
        MoveSetState(EnemyMoveState.stop);
    }

    void Update()
    {
        if (enemyStatus.IsDead) return; //HPが0、または追いかける対象が見つからない場合

        if(enemyStatus.IsRagdoll) return; //ラグドール状態のときは移動しない

        if (moveType == EnemyMoveType.StageDestroy && lookTarget == null)
            InitTarget();

        if (lookTarget == null) return;
        
        float distance = Vector3.Distance(transform.position, lookTarget.position); //プレイヤーとの距離を算出する

        moveTypeTimer += Time.deltaTime;
        if (moveTypeTimer >= distanceCheckInterval) //一定間隔ごとに、移動タイプの更新
        {
            moveTypeTimer = 0f;
            UpdateMoveType(distance);
            CallStateEvent();
        }
    }

    void InitTarget() //移動タイプに応じてターゲットを設定する
    {
        switch (moveType)
        {
            case EnemyMoveType.PlayerChase: //プレイヤーの位置に関わらず、プレイヤーに向かって移動する

            case EnemyMoveType.BlockPlayer: //プレイヤーが一定距離まで近づいたときのみプレイヤーに向かって移動する
                GameObject player = GameObject.FindGameObjectWithTag("LookPoint");
                if (player != null) lookTarget = player.transform;
                break;

            case EnemyMoveType.StageDestroy:
                breakables = GameObject.FindGameObjectsWithTag("Breakable"); //攻撃できるオブジェクトに設定されているタグ名を()内に記述する
                if (breakables.Length > 0)
                    lookTarget =  GetNearestTarget(breakables); //一番近いオブジェクトに向かって移動する

                else return;
                break;
        }
    }

    void UpdateMoveType(float distance) //移動タイプに沿った、移動状態の遷移を行う
    {
        switch (moveType)
        {
            case EnemyMoveType.PlayerChase:
                if (distance <= stopRange) currentMoveState = EnemyMoveState.lookOnly;                    
                else currentMoveState = EnemyMoveState.move;
                break;

            case EnemyMoveType.BlockPlayer:
                if (distance > detectionRange) 
                    currentMoveState = EnemyMoveState.stop;
                else if (distance <= stopRange) currentMoveState = EnemyMoveState.lookOnly;
                else currentMoveState = EnemyMoveState.move;
                break;

            case EnemyMoveType.StageDestroy:
                if (lookTarget == null || !lookTarget.gameObject.activeInHierarchy)
                {
                    breakables = GameObject.FindGameObjectsWithTag("Breakable");
                    lookTarget = (breakables.Length > 0) ? GetNearestTarget(breakables) : null;
                }

                currentMoveState = (lookTarget != null && distance >= stopRange) ? EnemyMoveState.move : EnemyMoveState.lookOnly;
                break;
        }
    }

    public void MoveStateProcess() //移動状態ごとの移動処理を行う
    {
        if (enemyStatus.IsDead || enemyStatus.IsRagdoll) return;
        if (lookTarget == null) currentMoveState = EnemyMoveState.stop;

        switch (currentMoveState)
        {
            case EnemyMoveState.stop: //停止状態（プレイヤーを追従する必要がない）
                break;
            case EnemyMoveState.lookOnly: //プレイヤーの方向を向く処理のみ行う状態
                LookPlayer(lookTarget);
                break;
            case EnemyMoveState.move: //プレイヤーの方向を向いて追従する状態
                LookPlayer(lookTarget);
                MoveTowardsPlayer(lookTarget);
                break;
            default: break;
        }
    }

    void CallStateEvent() //移動状態ごとに、敵の行動状態の遷移を行う
    {
        switch (currentMoveState)
        {
            case EnemyMoveState.stop: //停止状態（プレイヤーを追従する必要がない）
                actionEvents.IdleEvent(); break;                

            case EnemyMoveState.move: //プレイヤーの方向を向いて追従する状態
                actionEvents.MoveEvent(); break;

            case EnemyMoveState.lookOnly: //プレイヤーの方向を向く処理のみ行う状態
            default: break;
        }
    }

    public void MoveTowardsPlayer(Transform target)//プレイヤーに向かって移動する
    {
        if (!isActiveMove || lookTarget == null) return;

        Vector3 targetPos = target.position;

        targetPos.y = transform.position.y;

        Vector3 direction = (targetPos - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void LookPlayer(Transform target)//Y軸だけ変える
    {
        if (!isActiveRotate || lookTarget == null) return;

        Vector3 playerPos = target.position;

        playerPos.y = lookTarget.transform.position.y;

        Vector3 direction = (playerPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    Transform GetNearestTarget(GameObject[] objects)
    {
        Transform nearest = null;

        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj.transform;
            }
        }

        return nearest;
    }

    public void ToggleActive(bool isActiveMove, bool isActiveRotate)
    {
        this.IsActiveMove = isActiveMove;
        this.IsActiveRotate = isActiveRotate;
    }
}
