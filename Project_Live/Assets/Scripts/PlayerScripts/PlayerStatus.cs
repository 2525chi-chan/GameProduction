using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

//敵の死亡判定の最適化
//いいねポイントをプレイヤーのパラメータに反映させる

public class PlayerStatus : CharacterStatus
{
    [Header("死亡時に発生するエフェクト")]
    [SerializeField] GameObject deathEffect;
    [Header("消滅させるオブジェクト")]
    [SerializeField] GameObject target;
    [Header("いいねポイント1つ獲得による、パラメータの上昇量の倍率")]
    [SerializeField] float addRatio = 0f;

    [Header("必要なコンポーネント")]
    [SerializeField] GoodSystem goodSystem;    

    public enum PlayerState
    {
        Normal, Invincible
    }

    PlayerState currentState = PlayerState.Normal;

    bool isDead = false;

    Rigidbody rb;

    float prev_goodNum = 0.1f;

    public PlayerState CurrentState { get { return currentState; } set { currentState = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    

    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
        //prev_goodNum = goodSystem.GoodNum;
    }

    void Update()
    {
        if (!isDead && Hp <= 0) Die();

        if (Hp <= 0) Hp = 0;

        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            currentState = currentState == PlayerState.Normal ? PlayerState.Invincible : PlayerState.Normal;
            Debug.Log(currentState + "状態に切り替わりました。");
        }

        //if (prev_goodNum != goodSystem.GoodNum)
        //{
        //    SetNewParameters();
        //    ShowParameters();
        //}

        //prev_goodNum = goodSystem.GoodNum;
    }

    void Die() //死亡時の処理
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
       
        
        Destroy(target);
        GameOverManager.Instance.StartGameOver();
    }

    void SetNewParameters() //いいねポイント獲得量に応じたパラメータの反映
    {
        AttackPower += goodSystem.GoodNum * addRatio; //攻撃力の変化
        Agility += goodSystem.GoodNum * addRatio; //移動速度の変化
    }

    void ShowParameters()
    {
        Debug.Log("HP:" + Hp);
        Debug.Log("AttackPower:" + AttackPower);
        Debug.Log("Agility:" +  Agility);
    }
}
