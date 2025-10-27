using Cinemachine;
using UnityEditor;
using UnityEngine;

public static class EnemyActionEvents
{
    public static event System.Action OnIdleEvent;
    public static event System.Action OnMoveEvent;
    public static event System.Action OnKnockbackEvent;
    public static event System.Action OnDownEvent;
    public static event System.Action OnCloseAttackEvent;
    public static event System.Action OnShotEvent;

    public static void IdleEvent()
    {
        OnIdleEvent?.Invoke();
    }

    public static void MoveEvent()
    {
        OnMoveEvent?.Invoke();
    }

    public static void KnockbackEvent()
    {
        OnKnockbackEvent?.Invoke();
    }

    public static void DownEvent()
    {
        OnDownEvent?.Invoke();
    }

    public static void CloseAttackEvent()
    {
        OnCloseAttackEvent?.Invoke();
    }

    public static void ShotEvent()
    {
        OnShotEvent?.Invoke();
    }
}
