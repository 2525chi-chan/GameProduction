using UnityEngine;

public class RushPhase
{
    GameObject rushHitbox;
    GameObject rushEffect;
    float rushDuration;
    float rushInterval;

    float elapsed = 0f;
    float intervalTimer = 0f;
    GameObject effectInstance;

    public bool IsFinished { get { return elapsed >= rushDuration; } }

    public RushPhase(GameObject hitbox, GameObject effect, float duration, float interval)
    {
        rushHitbox = hitbox;
        rushEffect = effect;
        rushDuration = duration;
        rushInterval = interval;
    }

    public void Start() //�����ݒ�
    {
        elapsed = 0f;
        intervalTimer = 0f;
        if (rushHitbox != null) rushHitbox.SetActive(false);
    }

    public void Update() //���b�V���U�����̏���
    {
        if (IsFinished) return;

        elapsed += Time.deltaTime;
        intervalTimer += Time.deltaTime;

        // ��x�����G�t�F�N�g�������s��
        if (rushEffect != null && effectInstance == null)
            effectInstance = GameObject.Instantiate(rushEffect, rushHitbox.transform.position, rushHitbox.transform.rotation);

        // ���Ԋu�œ����蔻���ON/OFF�؂�ւ�����
        if (intervalTimer >= rushInterval)
        {
            ToggleHitbox(rushHitbox);
            intervalTimer = 0f;
        }
    }

    public void End() //�I������
    {
        if (!rushHitbox.activeSelf) return;
        if (rushHitbox != null) rushHitbox.SetActive(false);
    }

    void ToggleHitbox(GameObject hitbox) //�����蔻��̐؂�ւ�
    {
        if (hitbox == null) return;
        hitbox.SetActive(!hitbox.activeSelf);
    }
}
