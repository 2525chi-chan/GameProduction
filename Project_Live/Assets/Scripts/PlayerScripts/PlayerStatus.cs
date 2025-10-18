using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerStatus : CharacterStatus
{
    [Header("死亡時に発生するエフェクト")]
    [SerializeField] GameObject deathEffect;
    [Header("消滅させるオブジェクト")]
    [SerializeField] GameObject target;
    

    public enum PlayerState
    {
        Normal, Invincible
    }

    PlayerState currentState = PlayerState.Normal;

    bool isDead = false;

    Rigidbody rb;

    public PlayerState CurrentState { get { return currentState; } set { currentState = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    

    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
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
    }

    void Die() //死亡時の処理
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
       
        
        Destroy(target);
        GameOverManager.Instance.StartGameOver();
    }

   
}
