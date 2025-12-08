using Cinemachine;
using UnityEditor;
using UnityEngine;

public class EnemyActionEvents
{
    public event System.Action OnIdleEvent;
    public event System.Action OnMoveEvent;
    public event System.Action OnKnockbackEvent;
    public event System.Action OnDownEvent;
    public event System.Action OnAttackEvent;
    public event System.Action OnBossAttackStartEvent;
    public event System.Action OnBossAttackFinishEvent;

    public void IdleEvent()
    {
        OnIdleEvent?.Invoke();
    }

    public void MoveEvent()
    {
        OnMoveEvent?.Invoke();
    }

    public void KnockbackEvent()
    {
        OnKnockbackEvent?.Invoke();
    }

    public void DownEvent()
    {
        OnDownEvent?.Invoke();
    }

    public void AttackEvent()
    {
        OnAttackEvent?.Invoke();
    }

    public void BossAttackStartEvent()
    {
        OnBossAttackStartEvent?.Invoke();
    }

    public void BossAttackFinishEvent()
    {
        OnBossAttackFinishEvent?.Invoke();
    }
}
