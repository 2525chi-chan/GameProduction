using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStatus : CharacterStatus
{   
    public EnemyActionEvents actionEvents = new EnemyActionEvents();
    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    float previousHp;

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

    float ragdollCount;

    Transform pos;
    private void Start()
    {
        pos = this.transform;
        previousHp = Hp;
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

        if (Hp != previousHp && Hp > 0) actionEvents.KnockbackEvent();

        previousHp = Hp;
    }
}
