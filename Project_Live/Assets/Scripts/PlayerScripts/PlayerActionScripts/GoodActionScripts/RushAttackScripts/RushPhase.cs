using Cinemachine;
using UnityEngine;

public class RushPhase
{
    GameObject rushHitbox;
    GameObject rushEffect;
    float rushDuration;
    float rushInterval;
    Transform followTarget;

    float elapsed = 0f;
    float intervalTimer = 0f;
    GameObject effectInstance;

    public bool IsFinished { get { return elapsed >= rushDuration; } }

    public RushPhase(GameObject hitbox, GameObject effect, float duration, float interval, Transform target)
    {
        rushHitbox = hitbox;
        rushEffect = effect;
        rushDuration = duration;
        rushInterval = interval;
        followTarget = target;
    }

    public void Start() //初期設定
    {
        elapsed = 0f;
        intervalTimer = 0f;
        if (rushHitbox != null) rushHitbox.SetActive(false);
    }

    public void Update() //ラッシュ攻撃中の処理
    {
        if (IsFinished) return;

        elapsed += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        // 一度だけエフェクト生成を行う
        if (rushEffect != null && effectInstance == null)
            effectInstance = GameObject.Instantiate(rushEffect, rushHitbox.transform.position, rushHitbox.transform.rotation);


        if (followTarget != null && effectInstance != null)
            effectInstance.transform.SetParent(followTarget);

        // 一定間隔で当たり判定をON/OFF切り替えする
        if (intervalTimer >= rushInterval)
        {
            ToggleHitbox(rushHitbox);
            intervalTimer = 0f;
        }
    }

    public void End() //終了処理
    {
        if (!rushHitbox.activeSelf) return;
        if (rushHitbox != null) rushHitbox.SetActive(false);
    }

    void ToggleHitbox(GameObject hitbox) //当たり判定の切り替え
    {
        if (hitbox == null) return;
        hitbox.SetActive(!hitbox.activeSelf);
    }
}
