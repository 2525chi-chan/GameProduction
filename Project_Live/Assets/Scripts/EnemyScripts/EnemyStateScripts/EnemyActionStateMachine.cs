using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyActionStateMachine : MonoBehaviour
{
    IEnemyState currentState; //現在の状態格納用

    [Tooltip("必要なコンポーネント")]
    [SerializeField] EnemyAnimationController anim;
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] AttackController attackController;
    [SerializeField] AttackTrigger attackTrigger;

    public EnemyActionEvents actionEvents = new EnemyActionEvents();
    
    public IEnemyState CurrentState { get { return currentState; } }

    void Awake()
    {
        status.actionEvents = actionEvents;
        mover.actionEvents = actionEvents;
        attackTrigger.actionEvents = actionEvents;
        ChangeState(new IdleState_Enemy(anim, status, mover));
    }

    void OnEnable()
    {
        actionEvents.OnIdleEvent += OnIdleProcess;
        actionEvents.OnMoveEvent += OnMoveProcess;
        actionEvents.OnKnockbackEvent += OnKnockbackProcess;
        actionEvents.OnDownEvent += OnDownProcess;
        actionEvents.OnAttackEvent += OnAttackProcess;
    }

    void OnDisable()
    {
        actionEvents.OnIdleEvent -= OnIdleProcess;
        actionEvents.OnMoveEvent -= OnMoveProcess;
        actionEvents.OnKnockbackEvent -= OnKnockbackProcess;
        actionEvents.OnDownEvent -= OnDownProcess;
        actionEvents.OnAttackEvent -= OnAttackProcess;
    }

    void Update()
    {
        currentState?.Update();
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
        ChangeState(new IdleState_Enemy(anim, status, mover));
    }

    void OnMoveProcess()
    {
        if (currentState is IdleState_Enemy && !(currentState is MoveState_Enemy)) 
            ChangeState(new MoveState_Enemy(anim, mover));
    }

    void OnKnockbackProcess()
    {
        if (currentState is KnockbackState_Enemy) return;
        ChangeState(new KnockbackState_Enemy(anim));
    }

    void OnDownProcess()
    {
        if (currentState is DownState_Enemy) return;
        ChangeState(new DownState_Enemy(anim));
    }

    void OnAttackProcess()
    {
        if (currentState is AttackState_Enemy) return;
        //if ((currentState is IdleState_Enemy || currentState is MoveState_Enemy) && !(currentState is AttackState_Enemy))
            ChangeState(new AttackState_Enemy(anim, mover, attackController));
    }
}
