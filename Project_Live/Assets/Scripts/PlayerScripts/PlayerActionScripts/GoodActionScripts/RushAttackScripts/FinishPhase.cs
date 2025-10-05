using UnityEngine;

public class FinishPhase
{
    GameObject finishHitbox;
    GameObject finishEffect;
    float delay;
    float duration;

    float delayTimer = 0f;
    float elapsed = 0f;
    bool isActive = false;

    public bool IsFinished { get; private set; } = false;

    public FinishPhase(GameObject hitbox, GameObject effect, float delay, float duration)
    {
        finishHitbox = hitbox;
        finishEffect = effect;
        this.delay = delay;
        this.duration = duration;
    }

    public void Start() //初期設定
    {
        delayTimer = 0f;
        elapsed = 0f;
        isActive = false;
        IsFinished = false;
        if (finishHitbox != null) finishHitbox.SetActive(false);
    }

    public void Update() //フィニッシュ攻撃中の処理
    {
        if (IsFinished) return;

        if (!isActive)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delay) Activate();
        }

        else
        {
            elapsed += Time.deltaTime;
            if (elapsed >= duration) End();
        }
    }

    void Activate() //フィニッシュ攻撃を開始するための処理
    {
        isActive = true;
        if (finishHitbox != null) finishHitbox.SetActive(true);
        if (finishEffect != null) GameObject.Instantiate(finishEffect, finishHitbox.transform.position, finishHitbox.transform.rotation);
        elapsed = 0f;
    }

    void End() //終了処理
    {
        if (finishHitbox != null) finishHitbox.SetActive(false);
        IsFinished = true;
    }
}
