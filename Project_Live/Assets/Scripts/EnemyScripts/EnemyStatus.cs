using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStatus : CharacterStatus
{   
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

    float ragdollCount;
   
    private void Start()
    {
        
    }
    private void Update()
    {
        if (deathHandler == null) return;

        if (Hp <= 0 && !deathHandler.IsProcessing) //HPが0になった、かつ死亡時の処理が行われていない場合
        {
            isDead = true;
            deathHandler.StartDeathProcess(); //死亡時の処理を開始する
        }
        if (!IsRagdoll&&deathHandler.IsGrounded())
        {
            ragdollCount += Time.deltaTime;


            if (ragdollCount > returnRagdollTime)
            {
                this.GetComponent<EnemyRagdoll>().SwitchRagdoll(true);
                ragdollCount = 0;
            }
        }
        else
        {
            ragdollCount = 0;
        }
    }
}
