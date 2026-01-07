using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStatus : CharacterStatus
{
    EnemyActionStateMachine stateMachine;
    public EnemyActionEvents actionEvents = new EnemyActionEvents();
    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    bool isRagdoll = false;
    public bool IsRagdoll
    {
        get { return isRagdoll; }
        set { isRagdoll = value; }
    }
    

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyDeathHandler deathHandler;
    [Header("地面についてからラグドール状態を解除するまでの時間")]
    [SerializeField]float returnRagdollTime = 1f;
    [Header("のけぞるまでの被ダメージ回数")]
    [SerializeField] int knockbackThresholdCount = 1;
    [Header("のけぞりから回復する時間")]
    [SerializeField] float knockbackRecoveryTime = 1.0f;

    float ragdollCount;

    Transform pos;

    int currentDamageCount = 0;

    public int CurrentDamageCount { get { return currentDamageCount; } set { currentDamageCount = value; } }
    public int KnockbackThresholdCount { get { return knockbackThresholdCount; } set { knockbackThresholdCount = value; } }
    public float KnockbackRecoveryTime { get { return knockbackRecoveryTime; } }

    public bool isTarget;

    private void Start()
    {
        pos = this.transform;
        if (stateMachine == null)
            stateMachine = GetComponentInParent<EnemyActionStateMachine>();
    }
    void Update()
    {
        if (deathHandler == null) return;

        if (Hp <= 0 && !deathHandler.IsProcessing) //HPが0になった、かつ死亡時の処理が行われていない場合
        {
            isDead = true;
            deathHandler.StartDeathProcess(); //死亡時の処理を開始する
            actionEvents.DownEvent(); //ダウン状態に移行する
        }
        if (IsRagdoll&&deathHandler.IsGrounded()&&!isDead)
        {
            ragdollCount += Time.deltaTime;


            if (ragdollCount > returnRagdollTime)
            {
                this.GetComponent<EnemyRagdoll>().SwitchRagdoll(false);
                ragdollCount = 0;
                Debug.Log("かいじょ");
                actionEvents.IdleEvent();
            }
           
        }
        else
        {
            ragdollCount = 0;
        }

        if (currentDamageCount >= knockbackThresholdCount)
        {
            currentDamageCount = 0;
            actionEvents.KnockbackEvent(); //のけぞり状態への遷移
        }
    }
}
