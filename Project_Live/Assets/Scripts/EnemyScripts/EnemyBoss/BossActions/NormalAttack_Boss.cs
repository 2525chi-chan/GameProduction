using System.Threading;
using UnityEngine;

public class NormalAttack_Boss : MonoBehaviour
{
    EnemyActionStateMachine stateMachine;
    public enum AttackState { Search, Idle, ShowAttackArea, Attack, Cooldown, Exit}
    
    [Header("攻撃判定を持つオブジェクト")]
    [SerializeField] GameObject attackPrefab;
    [Header("攻撃を生成する位置")]
    [SerializeField] Transform attackPosition;
    [Header("攻撃するまでの待ち時間")]
    [SerializeField] float attackDuration = 3f;
    [Header("攻撃判定の持続時間")]
    [SerializeField] float attackEnabledTime = 0.2f;
    [Header("攻撃後のクールタイム")]
    [SerializeField] float attackCoolTime = 1f;

    [Header("攻撃を始める判定を行う領域")]
    [SerializeField] Collider triggerRange;

    [Header("攻撃を行うまでに必要な判定時間")]
    [SerializeField] float triggerDuration = 0.8f;

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] Animator animator;
    [SerializeField] AttackWarningController attackWarningController;

    AttackState currentAttackState = AttackState.Search;
    AttackState previousAttackState;

    GameObject attackObj;

    float currentTimer = 0f; //経過時間の測定用
    bool isActive = false; //このクラスの処理が有効か

    public bool IsActive { get { return isActive; } set { isActive = value; } }
    public AttackState CurrentAttackState { get { return currentAttackState; } set { currentAttackState = value; } }
    public AttackState PreviousAttackState { get { return previousAttackState; } }
    public void SetStartState()
    {
        currentAttackState = AttackState.Search;
        previousAttackState = currentAttackState;
    }

    void OnTriggerStay(Collider other) //攻撃判定を行うエリアにプレイヤーが侵入しているときの処理
    {
        if (status.IsDead || !isActive || currentAttackState != AttackState.Search) return; //敵のHPが0、探索状態でない場合は何もしない

        if (!other.CompareTag("Player")) return;

        currentTimer = 0f;

        currentAttackState = AttackState.Idle;
        //Debug.Log("攻撃待機状態に移行");
    }

    public void StateProcess() //ステートごとの処理
    {
        if (stateMachine == null)
            stateMachine = GetComponentInParent<EnemyActionStateMachine>();

        if (status.IsDead || !isActive) return;

        switch (currentAttackState)
        {
            case AttackState.Search:
                mover.MoveStateProcess();
                break;

            case AttackState.Idle: //予告表示を行うまでの待機状態
                currentTimer += Time.deltaTime;
                //プレイヤーを感知し、予告表示を行うまでの時間
                if (currentTimer >= triggerDuration)
                {
                    currentAttackState = AttackState.ShowAttackArea;
                    currentTimer = 0f;
                    //Debug.Log("攻撃予告状態に移行");
                    //actionEvents.AttackEvent();
                    mover.ToggleActive(false, false);
                }
                break;

            case AttackState.ShowAttackArea: //攻撃予告を表示している状態
                if (!attackWarningController.IsWarningActive && !attackWarningController.IsWarningFinished)
                    attackWarningController.ShowAttackWarning(attackPosition);

                if (attackWarningController.IsWarningFinished)
                {
                    currentAttackState = AttackState.Attack;
                    currentTimer = 0f;
                    //Debug.Log("攻撃状態に移行");
                    attackWarningController.IsWarningFinished = false;
                }
                break;

            case AttackState.Attack: //攻撃状態
                currentTimer += Time.deltaTime;

                if (currentTimer >= attackDuration)
                {
                    InstanceAttack();
                    currentTimer = 0f;
                    currentAttackState = AttackState.Cooldown;
                    //Debug.Log("クールダウン状態に移行");
                }
                break;

            case AttackState.Cooldown: //クールダウン状態
                currentTimer += Time.deltaTime;

                if (currentTimer >= attackCoolTime)
                {
                    currentTimer = 0f;
                    currentAttackState = AttackState.Idle;
                    //Debug.Log("攻撃待機状態に移行");

                    mover.ToggleActive(true, true);
                    stateMachine?.actionEvents?.BossAttackFinishEvent();
                }
                break;

            case AttackState.Exit: //別の攻撃方法に移行時に呼ぶ処理
                mover.ToggleActive(true, true);
                stateMachine?.actionEvents?.BossAttackStartEvent();
                isActive = false;
                break;
        }

        if (previousAttackState != currentAttackState)
            previousAttackState = currentAttackState;
    }


    public void InstanceAttack() //攻撃処理
    {
        attackObj = Instantiate(attackPrefab, attackPosition.position, attackPosition.rotation);
        Destroy(attackObj, attackEnabledTime);
        
        if (mover != null)
        {
            var hitbox = attackObj.GetComponent<HitboxTrigger>();

            if (hitbox != null)
                hitbox.SetOwnerMoveType(EnemyMover.EnemyMoveType.PlayerChase); //攻撃判定に、攻撃者の移動タイプを渡す
        }
    }
}
