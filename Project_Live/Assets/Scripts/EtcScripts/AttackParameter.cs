using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParameter : MonoBehaviour
{
    [Header("基本ダメージ")]
    [SerializeField] float baceDamage = 5f;
    [Header("命中時に発生させるエフェクト")]
    [SerializeField] GameObject hitEffect;
    [Header("基本となる前方向への吹き飛ばし力")]
    [SerializeField] float baceForwardKnockbackForce = 1f;
    [Header("基本となる上方向への吹き飛ばし力")]
    [SerializeField] float baceUpwardKnockbackForce = 1f;
    [Header("基本となる下方向への吹き飛ばし力")]
    [SerializeField] float baceDownwardKnockbackForce = 0f;
    [Header("常に攻撃者の正面方向に吹き飛ぶ力を加えるか")]
    [SerializeField] bool enableForward = false;
    [Header("攻撃判定の中心に引き寄せる力を有効にするか")]
    [SerializeField] bool enableSuction = false;
    [Header("引き寄せる力")]
    [SerializeField] float suctionForce = 10f;
    //[Header("取得するコンポーネントのオブジェクト名")]
    //[SerializeField] string objectName = "PlayerStatus";
    [Header("必要なコンポーネント")]
    [SerializeField] DamageToTarget damageToTarget;

    GameObject target;
    PlayerStatus status;

    void Start()
    {
        SetParameters();
    }

    void SetParameters() //ダメージ、命中時のエフェクト、吹き飛ばし力を設定する
    {
        damageToTarget.Damage = GetDamage();
        damageToTarget.HitEffect = hitEffect;
        damageToTarget.ForwardKnockbackForce = GetForwardForce();
        damageToTarget.UpwardKnockbackForce = GetUpwardForce();
        damageToTarget.DownwardKnockbackForce = GetDownwardForce();
        damageToTarget.EnableSuction = enableSuction;
        damageToTarget.SuctionForce = suctionForce;

    }

    float GetDamage() //ダメージ量を取得する
    {
        return target != null ? baceDamage * status.AttackPower : baceDamage;
    }

    float GetForwardForce() //上方向への吹き飛ばし力を取得する
    {
        return target != null ? baceForwardKnockbackForce * status.AttackPower : baceForwardKnockbackForce;
    }

    float GetUpwardForce() //前方向への吹き飛ばし力を取得する
    {
        return target != null ? baceUpwardKnockbackForce * status.AttackPower : baceUpwardKnockbackForce;
    }

    float GetDownwardForce() //下方向への吹き飛ばし力を取得する
    {
        return target != null ? baceDownwardKnockbackForce * status.AttackPower : baceDownwardKnockbackForce;
    }
}
