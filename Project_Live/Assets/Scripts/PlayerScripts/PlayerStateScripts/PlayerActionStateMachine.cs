using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//作成者：桑原

public class PlayerActionStateMachine : MonoBehaviour //プレイヤーの行動状態の管理を行う
{
    IPlayerState currentState;
    PlayerAnimationController anim;

    [Header("必要なコンポーネント")]
    [SerializeField] PlayerAnimationController animController;
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] MovePlayer movePlayer;
    [SerializeField] CloseAttack closeAttack;
    [SerializeField] ShotAttack shotAttack;
    [SerializeField] Dodge dodge;
    [SerializeField] GoodAction goodAction;

    public IPlayerState CurrentState { get { return currentState; } }    

    void Awake()
    {
        anim = animController.GetComponent<PlayerAnimationController>();
        ChangeState(new IdleState(anim));
    }

    void OnEnable()
    {
        PlayerActionEvents.OnIdleEvent += OnIdleInput;
        PlayerActionEvents.OnMoveEvent += OnMoveInput;
        PlayerActionEvents.OnCloseAttackEvent += OnCloseAttackInput;
        PlayerActionEvents.OnShotEvent += OnShotInput;
        PlayerActionEvents.OnDodgeEvent += OnDodgeInput;
        PlayerActionEvents.OnGoodAction1Event += OnGoodAction1Input;
        PlayerActionEvents.OnGoodAction2Event += OnGoodAction2Input;
        PlayerActionEvents.OnGoodAction3Event += OnGoodAction3Input;
        PlayerActionEvents.OnGoodAction4Event += OnGoodAction4Input;
    }

    void OnDisable()
    {
        PlayerActionEvents.OnIdleEvent -= OnIdleInput;
        PlayerActionEvents.OnMoveEvent -= OnMoveInput;
        PlayerActionEvents.OnCloseAttackEvent -= OnCloseAttackInput;
        PlayerActionEvents.OnShotEvent -= OnShotInput;
        PlayerActionEvents.OnDodgeEvent -= OnDodgeInput;
        PlayerActionEvents.OnGoodAction1Event -= OnGoodAction1Input;
        PlayerActionEvents.OnGoodAction2Event -= OnGoodAction2Input;
        PlayerActionEvents.OnGoodAction3Event -= OnGoodAction3Input;
        PlayerActionEvents.OnGoodAction4Event -= OnGoodAction4Input;
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void OnIdleInput()
    {
        if (!(currentState is IdleState)) ChangeState(new IdleState(anim));
    }

    void OnMoveInput()
    {
        if (currentState is IdleState && !(currentState is MoveState)) ChangeState(new MoveState(anim, movePlayer));
    }

    void OnCloseAttackInput()
    {
        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is CloseAttackState))
        {
            ChangeState(new CloseAttackState(anim, movePlayer, closeAttack));
        }
    }

    void OnShotInput()
    {
        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is ShotState))
        {
            ChangeState(new ShotState(anim, movePlayer, shotAttack));
        }
    }

    void OnDodgeInput()
    {
        if (dodge.IntervalTimer < dodge.DodgeInterval) return;

        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is DodgeState))
        {
            ChangeState(new DodgeState(anim, movePlayer, dodge));
        }
    }

    void OnGoodAction1Input()
    {
        if (goodAction.CurrentGoodPoint1 < goodAction.GoodAction1Parameters.GoodCost) return;

        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is GoodAction1State))
        {
            ChangeState(new GoodAction1State(anim, playerStatus, goodAction));
        }
    }

    void OnGoodAction2Input()
    {
        if (goodAction.CurrentGoodPoint2 < goodAction.GoodAction2Parameters.GoodCost) return;

        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is GoodAction2State))
        {
            ChangeState(new GoodAction2State(anim, playerStatus, goodAction));
        }
    }

    void OnGoodAction3Input()
    {
        if (goodAction.CurrentGoodPoint3 < goodAction.GoodAction3Parameters.GoodCost) return;

        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is GoodAction3State))
        {
            ChangeState(new GoodAction3State(anim, playerStatus, goodAction));
        }
    }

    void OnGoodAction4Input()
    {
        if (goodAction.CurrentGoodPoint4 < goodAction.GoodAction4Parameters.GoodCost) return;

        if ((currentState is IdleState || currentState is MoveState)
            && !(currentState is GoodAction4State))
        {
            ChangeState(new GoodAction4State(anim, playerStatus, goodAction));
        }
    }
}
