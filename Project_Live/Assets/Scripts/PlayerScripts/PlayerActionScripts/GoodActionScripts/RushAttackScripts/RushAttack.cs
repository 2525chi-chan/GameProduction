using UnityEngine;

public class RushAttack : MonoBehaviour
{
    [Header("���b�V���ݒ�")]
    [Tooltip("���b�V���U���̓����蔻��")]
    [SerializeField] GameObject rushHitbox;
    [Tooltip("���b�V���U���J�n���ɐ�������G�t�F�N�g")]
    [SerializeField] GameObject rushEffect;
    [Tooltip("���b�V���U���̌p������")]
    [SerializeField] float rushDuration = 2f;
    [Tooltip("���b�V���U���̔����Ԋu")]
    [SerializeField] float rushInterval = 0.1f;

    [Header("�t�B�j�b�V���ݒ�")]
    [Tooltip("�t�B�j�b�V���U���̓����蔻��")]
    [SerializeField] GameObject finishHitbox;
    [Tooltip("�t�B�j�b�V���U���������ɐ�������G�t�F�N�g")]
    [SerializeField] GameObject finishEffect;
    [Tooltip("���b�V���U���I����A�t�B�j�b�V���U���𔭓�����܂ł̎���")]
    [SerializeField] float finishDelay = 0.5f;
    [Tooltip("�t�B�j�b�V���U���̎�������")]
    [SerializeField] float finishDuration = 0.5f;

    RushPhase rushPhase;
    FinishPhase finishPhase;
    bool isActive = false;

    void Start()
    {
        rushPhase = new RushPhase(rushHitbox, rushEffect, rushDuration, rushInterval);
        finishPhase = new FinishPhase(finishHitbox, finishEffect, finishDelay, finishDuration);

        SetHitboxesActive(false);
    }

    void Update()
    {
        if (!isActive) return;

        rushPhase.Update();

        if (rushPhase.IsFinished)
        {
            rushPhase.End();
            finishPhase.Update();

            if (finishPhase.IsFinished) ResetState();
        }
    }

    public void Activate() //���b�V���U�����J�n���邽�߂ɌĂԏ���
    {
        isActive = true;
        rushPhase.Start();
        finishPhase.Start();
    }

    public void ResetState() //�t���O�ⓖ���蔻��̏�����
    {
        isActive = false;
        SetHitboxesActive(false);
    }

    void SetHitboxesActive(bool active) //�����蔻��̗L�E�������؂�ւ�
    {
        if (rushHitbox != null) rushHitbox.SetActive(active);
        if (finishHitbox != null) finishHitbox.SetActive(active);
    }
}
