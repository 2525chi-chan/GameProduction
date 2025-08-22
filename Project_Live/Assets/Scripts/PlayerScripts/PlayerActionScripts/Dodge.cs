using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class Dodge : MonoBehaviour
{
    [Header("回避させる対象")]
    [SerializeField] Transform target;
    [Header("1回ごとの回避時間")]
    [SerializeField] float dodgeDuration = 0.5f;
    [Header("回避状態解除後、次にできるようになるまでの時間")]
    [SerializeField] float dodgeInterval = 0.5f;
    [Header("回避による移動速度の加速量")]
    [SerializeField] float dodgeSpeed = 20f;
    [Header("回避中であることを示すエフェクト")]
    [SerializeField] ParticleSystem dodgeEffect;

    [Header("必要なコンポーネント")]
    [SerializeField] PlayerStatus status;

    float dodgeTimer = 0f; //回避状態に移行してからの経過時間計測用
    float intervalTimer = 0f; //回避状態解除後の経過時間計測用
    bool isDodging = false; //回避中かどうか

    public float DodgeInterval { get { return dodgeInterval; } }
    public float IntervalTimer { get { return intervalTimer; } }
    public bool IsDodging { get { return isDodging; } }

    void Start()
    {
        dodgeTimer = dodgeInterval; //初回のみ、時間を待たずに回避できるようにする

        //Debug.Log(dodgeTimer);
    }
    void Update()
    {
        if (isDodging) return;

        if (intervalTimer < dodgeInterval) intervalTimer += Time.deltaTime;
    }

    public void TryDodge() //回避の始動処理
    {
        if (isDodging) return;

        isDodging = true; //回避中
        status.CurrentState = PlayerStatus.PlayerState.Invincible; //プレイヤーを無敵状態にする
        dodgeEffect.Play();
        dodgeTimer = 0f;
    }

    public void DodgeProcess() //回避処理
    {
        if (isDodging)
        {
            DodgeMove();

            dodgeTimer += Time.deltaTime;

            if (dodgeTimer >= dodgeDuration)
            {
                isDodging = false;
                dodgeTimer = 0f;
                intervalTimer = 0f;
                status.CurrentState = PlayerStatus.PlayerState.Normal; //プレイヤーの無敵状態を解除する
                dodgeEffect.Stop();
                PlayerActionEvents.IdleEvent(); //待機状態に移行する
            }
        }
    }

    void DodgeMove() //回避中の移動処理
    {
        if (target == null || !isDodging) return; //移動させる対象が設定されていないか、回避中でない場合は何も行わない

        target.position += target.forward  * dodgeSpeed * Time.deltaTime;
    }
}
