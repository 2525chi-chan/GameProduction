using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStatus : CharacterStatus
{   
    bool isDead = false;
    public bool IsDead { get { return isDead; } }


    [Header("必要なコンポーネント")]
    [SerializeField] EnemyDeathHandler deathHandler;

    private void Update()
    {
        if (Hp <= 0 && !deathHandler.IsProcessing) //HPが0になった、かつ死亡時の処理が行われていない場合
        {
            isDead = true;
            deathHandler.StartDeathProcess(); //死亡時の処理を開始する
        }
    }
}
