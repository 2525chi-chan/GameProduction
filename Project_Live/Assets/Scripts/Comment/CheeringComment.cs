//制作者　寺村

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheeringComment  :ReplyCommentBase
{
    [Header("プレイヤーの最大HPの何%分HPを加えるか")]
    [SerializeField,Range(1,100)] int addHPPercent;
    [Header("HPが何%以下になったら回復にするか")]
    [SerializeField,Range(1,100)] float hpPercent;
    [Header("バフする攻撃倍率")]
    [SerializeField] float attackMagnification;
    [Header("バフする速度倍率")]
    [SerializeField] float agilityMagnification;
    [Header("バフの持続時間")]
    [SerializeField] float buffTime;
    [Header("このコメントのサウンド")]
    [SerializeField] AudioClip commentSound;

    AudioSource SE;

    PlayerStatus playerStatus;
    PlayerBuffManager playerBuffManager;

    GameObject PowerBuffImage;
    GameObject AgilityBuffImage;

    void Start()
    {
        //返信コメント共通の準備部分
        InitializeAnything();

        SE = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
        PowerBuffImage = GameObject.FindGameObjectWithTag("PowerBuffImage");
        AgilityBuffImage = GameObject.FindGameObjectWithTag("AgilityBuffImage");
        playerBuffManager = GameObject.FindGameObjectWithTag("PlayerBuffManager").GetComponent<PlayerBuffManager>();
        playerStatus = GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        commentSpawn.cheeringCommentIsExist = true;

        SetCommentAction();
    }

    void Update()
    {
        //返信コメント共通のエリア判定部
        CheckInArea();
    }
    private void SetCommentAction()     //応援コメントの効果設定関数
    {
        thisButton.onClick.RemoveAllListeners();    // 念のためリスナーをクリア

        if (playerStatus.Hp / playerStatus.MaxHp <= hpPercent/100)  // HPが指定%以下なら回復
        {
            thisButton.onClick.AddListener(AddHP);
        }
        else
        {
            //  HPが指定%以上ならバフ処理
            bool attackActive = playerBuffManager.attackBuff != null && playerBuffManager.attackBuff.isActive;
            bool speedActive = playerBuffManager.speedBuff != null && playerBuffManager.speedBuff.isActive;

            if (attackActive && !speedActive)
            {
                // 攻撃バフ中ならスピードバフ
                thisButton.onClick.AddListener(BuffAgility);
            }
            else if (!attackActive && speedActive)
            {
                // スピードバフ中なら攻撃バフ
                thisButton.onClick.AddListener(BuffAtack);
            }
            else
            {
                // 両方かかっている or 両方かかっていない場合はランダム
                if (Random.value < 0.5f)
                    thisButton.onClick.AddListener(BuffAtack);
                else
                    thisButton.onClick.AddListener(BuffAgility);
            }
        }
    }

    public void AddHP()     //HP回復効果
    {
        SE.PlayOneShot(commentSound);
        playerBuffManager.AddHP(addHPPercent);
        DestroyComment();
    }

    public void BuffAtack()     //攻撃バフ効果
    {
        AnimationBeforeMethod();
        SE.PlayOneShot(commentSound);

        // コルーチン完了後にBuffAttackを実行するコールバックを渡す
        OnButtonClickedAnimation(PowerBuffImage, () =>
        {
            // アニメーション完了後に実行される
            playerBuffManager.BuffAttack(attackMagnification, buffTime);
            DestroyComment();
        });
    }

    public void BuffAgility()   //速度バフ効果
    {
        AnimationBeforeMethod();
        SE.PlayOneShot(commentSound);

        OnButtonClickedAnimation(AgilityBuffImage, () =>
        {
            playerBuffManager.BuffMoveSpeed(agilityMagnification, buffTime);
            DestroyComment();
        });
    }

    void DestroyComment()
    {
        UnregisterReplyList();
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        commentSpawn.cheeringCommentIsExist = false;
    }
}
