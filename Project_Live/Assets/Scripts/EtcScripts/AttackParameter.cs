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
    [Header("取得するコンポーネントのオブジェクト名")]
    [SerializeField] string objectName = "PlayerStatus";
    [Header("必要なコンポーネント")]
    [SerializeField] DamageToTarget damageToTarget;

    GameObject owner;
    
    CharacterStatus status;

    public void SetOwner(GameObject ownerObj) //攻撃を生成した主を設定する
    {
        owner = ownerObj;
        status = owner.GetComponent<CharacterStatus>();
        SetParameters();

        //if (status != null) Debug.Log("パラメータを取得した");
    }

    void SetParameters() //ダメージ、命中時のエフェクト、吹き飛ばし力を設定する
    {
        if (owner != null) return;
        damageToTarget.Damage = GetDamage();
        damageToTarget.HitEffect = hitEffect;
        damageToTarget.ForwardKnockbackForce = GetForwardForce();
        damageToTarget.UpwardKnockbackForce = GetUpwardForce();
        damageToTarget.DownwardKnockbackForce = GetDownwardForce();
    }

    float GetDamage() //ダメージ量を取得する
    {
        return owner != null ? baceDamage * status.AttackPower : baceDamage;
    }

    float GetForwardForce() //上方向への吹き飛ばし力を取得する
    {
        return owner != null ? baceForwardKnockbackForce * status.AttackPower : baceForwardKnockbackForce;
    }

    float GetUpwardForce() //前方向への吹き飛ばし力を取得する
    {
        return owner != null ? baceUpwardKnockbackForce * status.AttackPower : baceUpwardKnockbackForce;
    }

    float GetDownwardForce() //下方向への吹き飛ばし力を取得する
    {
        return owner != null ? baceDownwardKnockbackForce * status.AttackPower : baceDownwardKnockbackForce;
    }
}
