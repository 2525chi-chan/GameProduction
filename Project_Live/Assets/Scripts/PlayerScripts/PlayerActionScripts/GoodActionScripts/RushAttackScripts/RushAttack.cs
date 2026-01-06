using UnityEngine;

public class RushAttack : MonoBehaviour
{
    [Header("ラッシュ設定")]
    [Tooltip("ラッシュ攻撃の当たり判定")]
    [SerializeField] GameObject rushHitbox;
    [Tooltip("ラッシュ攻撃開始時に生成するエフェクト")]
    [SerializeField] GameObject rushEffect;
    [Tooltip("ラッシュ攻撃の継続時間")]
    [SerializeField] float rushDuration = 2f;
    [Tooltip("ラッシュ攻撃の発生間隔")]
    [SerializeField] float rushInterval = 0.1f;

    [Header("フィニッシュ設定")]
    [Tooltip("フィニッシュ攻撃の当たり判定")]
    [SerializeField] GameObject finishHitbox;
    [Tooltip("フィニッシュ攻撃発生時に生成するエフェクト")]
    [SerializeField] GameObject finishEffect;
    [Tooltip("ラッシュ攻撃終了後、フィニッシュ攻撃を発動するまでの時間")]
    [SerializeField] float finishDelay = 0.5f;
    [Tooltip("フィニッシュ攻撃の持続時間")]
    [SerializeField] float finishDuration = 0.5f;

    RushPhase rushPhase;
    public FinishPhase finishPhase;
    bool isActive = false;

    void Start()
    {
        Transform effectFollowTarget = GameObject.Find("Player").transform;
        rushPhase = new RushPhase(rushHitbox, rushEffect, rushDuration, rushInterval, effectFollowTarget);
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

    public void Activate() //ラッシュ攻撃を開始するために呼ぶ処理
    {
        isActive = true;
        rushPhase.Start();
        finishPhase.Start();
    }

    public void ResetState() //フラグや当たり判定の初期化
    {
        isActive = false;
        SetHitboxesActive(false);
    }

    void SetHitboxesActive(bool active) //当たり判定の有・無効化切り替え
    {
        if (rushHitbox != null) rushHitbox.SetActive(active);
        if (finishHitbox != null) finishHitbox.SetActive(active);
    }
}
