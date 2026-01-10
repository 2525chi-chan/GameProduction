using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class BossStatus : MonoBehaviour
{
    [SerializeField] EnemyDeathHandler deathHandler;
    [SerializeField]GameClearManager gameClearManager;

    [Header("撃破時に生成するエフェクト")]
    [SerializeField] GameObject deathEffect;
    [Header("撃破時に出力する効果音")]
    [SerializeField] AudioClip deathSound;

    AudioSource SE;
    bool isActivedDeathEffect = false;

    private void Start()
    {
        if(deathHandler == null)
        {
            deathHandler = GetComponent<EnemyDeathHandler>();
        }
        if(gameClearManager == null)
        {
            gameClearManager = GameObject.FindWithTag("GameManager").GetComponent<GameClearManager>();
        }

        if (deathSound != null)
        {
            SE = GameObject.FindWithTag("SE").GetComponent<AudioSource>();
        }
    }
    public void Update()
    {
        if(deathHandler.IsDead)
        {
            // ボスが死亡したときの処理をここに追加
            Debug.Log("Boss has been defeated!");

            if (!isActivedDeathEffect)
            {
                isActivedDeathEffect = true;
                SE.PlayOneShot(deathSound);
                GameObject effect = GameObject.Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            gameClearManager.StartClearSequence();
        }
    }

}
