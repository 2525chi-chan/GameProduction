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

    public void Start() //�����ݒ�
    {
        delayTimer = 0f;
        elapsed = 0f;
        isActive = false;
        IsFinished = false;
        if (finishHitbox != null) finishHitbox.SetActive(false);
    }

    public void Update() //�t�B�j�b�V���U�����̏���
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

    void Activate() //�t�B�j�b�V���U�����J�n���邽�߂̏���
    {
        isActive = true;
        if (finishHitbox != null) finishHitbox.SetActive(true);
        if (finishEffect != null) GameObject.Instantiate(finishEffect, finishHitbox.transform.position, finishHitbox.transform.rotation);
        elapsed = 0f;
    }

    void End() //�I������
    {
        if (finishHitbox != null) finishHitbox.SetActive(false);
        IsFinished = true;
    }
}
