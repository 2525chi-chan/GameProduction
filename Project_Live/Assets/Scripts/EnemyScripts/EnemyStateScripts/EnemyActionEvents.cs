using Cinemachine;
using UnityEditor;
using UnityEngine;

public class EnemyActionEvents
{
    public event System.Action OnIdleEvent;
    public event System.Action OnMoveEvent;
    public event System.Action OnKnockbackEvent;
    public event System.Action OnDownEvent;
    public event System.Action OnCloseAttackEvent;
    public event System.Action OnShotEvent;

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

    public void CloseAttackEvent()
    {
        OnCloseAttackEvent?.Invoke();
    }

    public void ShotEvent()
    {
        OnShotEvent?.Invoke();
    }
}
