using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyActionStateMachine : MonoBehaviour
{
    IEnemyState currentState; //現在の状態格納用

    [Header("生成後の待機時間設定")]
    [Tooltip("ここで設定した時間が経過するまで、何もしない")]
    [SerializeField] float initialDelayTime = 0f;
    [Header("群れとボスのどちらの行動パターンを適用するか")]
    [SerializeField] bool isBossBehaviour = false;
    [Header("ステート遷移を順番かランダムのどちらで行うか")]
    [Tooltip("チェックをつけると順番、外すとランダムで状態が遷移する")]
    [SerializeField] bool useSequentialOrder = true;

    public enum BossAttackType { Normal, LongRange, Area }

    [System.Serializable]
    public class BossAttackQueue
    {
        [Header("通常攻撃")]
        public bool enableNormalAttack = true;
        [Header("遠距離攻撃")]
        public bool enableLongRangeAttack = true;
        [Header("範囲攻撃")]
        public bool enableAreaAttack = true;

        public List<BossAttackType> attackOrder = new List<BossAttackType>(); //順番指定用
    }
    [SerializeField] BossAttackQueue attackQueue = new BossAttackQueue();

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyAnimationController anim;
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] AttackController attackController;
    [SerializeField] AttackTrigger attackTrigger;
    [SerializeField] NormalAttack_Boss normalAttack;
    [SerializeField] LongRangeAttack_Boss longRangeAttack;
    [SerializeField] WideAreaAttack_Boss wideAreaAttack;

    public EnemyActionEvents actionEvents = new EnemyActionEvents();

    bool isActive = false;
    float currentTimer = 0f;
    bool isDelayFinished;

    public bool IsActive { get { return isActive; } set { isActive = value; } }
    
    public IEnemyState CurrentState { get { return currentState; } }
    public bool IsBossBehaviour { get { return isBossBehaviour; } }

    void Awake()
    {
        currentTimer = 0f;
        isDelayFinished = false;

        if (isBossBehaviour)
        {
            BuildBossAttackQueue();
        }

        status.actionEvents = actionEvents;
        mover.actionEvents = actionEvents;
        attackTrigger.actionEvents = actionEvents;
        ChangeState(new IdleState_Enemy(this, anim, status, mover));
    }

    void OnEnable()
    {
        actionEvents.OnIdleEvent += OnIdleProcess;
        actionEvents.OnMoveEvent += OnMoveProcess;
        actionEvents.OnKnockbackEvent += OnKnockbackProcess;
        actionEvents.OnDownEvent += OnDownProcess;
        actionEvents.OnAttackEvent += OnAttackProcess;
        actionEvents.OnBossAttackStartEvent += OnBossAttackStartProcess;
        actionEvents.OnBossAttackFinishEvent += OnBossAttackFinishProcess;
    }

    void OnDisable()
    {
        actionEvents.OnIdleEvent -= OnIdleProcess;
        actionEvents.OnMoveEvent -= OnMoveProcess;
        actionEvents.OnKnockbackEvent -= OnKnockbackProcess;
        actionEvents.OnDownEvent -= OnDownProcess;
        actionEvents.OnAttackEvent -= OnAttackProcess;
        actionEvents.OnBossAttackStartEvent -= OnBossAttackStartProcess;
        actionEvents.OnBossAttackFinishEvent -= OnBossAttackFinishProcess;
    }

    void BuildBossAttackQueue()
    {
        attackQueue.attackOrder.Clear();

        if (attackQueue.enableNormalAttack) attackQueue.attackOrder.Add(BossAttackType.Normal);
        if (attackQueue.enableLongRangeAttack) attackQueue.attackOrder.Add(BossAttackType.LongRange);
        if (attackQueue.enableAreaAttack) attackQueue.attackOrder.Add(BossAttackType.Area);

        if (!useSequentialOrder && attackQueue.attackOrder.Count > 1)
            ShuffleAttackOrder();
    }

    void ShuffleAttackOrder() //攻撃内容のシャッフル
    {
        for (int i = attackQueue.attackOrder.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, 0 + i);
            var temp = attackQueue.attackOrder[i];
            attackQueue.attackOrder[i] = attackQueue.attackOrder[randomIndex];
            attackQueue.attackOrder[randomIndex] = temp;
        }
    }

    IEnemyState GetNextBossAttack()
    {
        if (!isBossBehaviour || attackQueue.attackOrder.Count == 0)
            return new IdleState_Enemy(this, anim, status, mover);

        BossAttackType nextAttack = attackQueue.attackOrder[0];
        attackQueue.attackOrder.RemoveAt(0);

        if (attackQueue.attackOrder.Count == 0)
            BuildBossAttackQueue();

        return nextAttack switch
        {
            BossAttackType.Normal => new NormalAttackState_EnemyBoss(anim, mover, normalAttack),
            BossAttackType.LongRange => new LongRangeAttackState_EnemyBoss(anim, mover, longRangeAttack),
            BossAttackType.Area => new AreaAttackState_EnemyBoss(anim, mover, wideAreaAttack),
            _ => new IdleState_Enemy(this, anim, status, mover)
        };
    }

    void OnBossAttackStartProcess()
    {
        if (!isBossBehaviour || currentState is KnockbackState_Enemy
            || currentState is DownState_Enemy) return;

        ChangeState(GetNextBossAttack());
    }

    void OnBossAttackFinishProcess()
    {
        if (!isBossBehaviour) return;
        ChangeState(GetNextBossAttack());
    }

    void Update()
    {
        if (!isDelayFinished)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= initialDelayTime)
                isDelayFinished = true;

            return;
        }

        currentState?.Update();
        //Debug.Log(currentState);
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void OnIdleProcess()
    {
        if (currentState is IdleState_Enemy) return;
        ChangeState(new IdleState_Enemy(this, anim, status, mover));
    }

    void OnMoveProcess()
    {
        if (currentState is IdleState_Enemy && !(currentState is MoveState_Enemy)) 
            ChangeState(new MoveState_Enemy(anim, mover));
    }

    void OnKnockbackProcess()
    {
        if (currentState is KnockbackState_Enemy) return;
        ChangeState(new KnockbackState_Enemy(this, anim, status));
    }

    void OnDownProcess()
    {
        if (currentState is DownState_Enemy) return;
        ChangeState(new DownState_Enemy(anim));
    }

    void OnAttackProcess()
    {
        if (currentState is AttackState_Enemy) return;
        ChangeState(new AttackState_Enemy(anim, mover, attackController));
    }
}
