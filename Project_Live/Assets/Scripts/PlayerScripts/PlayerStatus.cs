using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerStatus : CharacterStatus
{
    [Header("���S���ɔ�������G�t�F�N�g")]
    [SerializeField] GameObject deathEffect;
    [Header("���ł�����I�u�W�F�N�g")]
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
            Debug.Log(currentState + "��Ԃɐ؂�ւ��܂����B");
        }
    }

    void Die() //���S���̏���
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
       
        
        Destroy(target);
        GameOverManager.Instance.StartGameOver();
    }

   
}
