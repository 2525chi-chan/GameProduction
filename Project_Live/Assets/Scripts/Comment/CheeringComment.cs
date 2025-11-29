//制作者　寺村

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheeringComment  :AnimationCheeringComment
{
    [Header("加えるHP")]
    [SerializeField] float addHPnum;
    [Header("HPが何%以下になったら回復にするか")]
    [SerializeField,Range(1,100)] float hpPercent;
    [Header("バフする攻撃倍率")]
    [SerializeField] float attackMagnification;
    [Header("バフする速度倍率")]
    [SerializeField] float agilityMagnification;
    [Header("バフの持続時間")]
    [SerializeField] float buffTime;
    [Header("応援コメントの内容")]
    public  List<string> cheeringCommentContent = new List<string>();
    [Header("決定時のエフェクト")]
    [SerializeField] ParticleSystem pressEffect;
    [Header("軌跡を描くエフェクト")]
    [SerializeField] GameObject trailEffect;
    [Header("軌跡エフェクトの有無")]
    [SerializeField] bool trailFlag;

    bool Pressed;

    PlayerStatus playerStatus;
    PlayerBuffManager playerBuffManager;
    CommentSpawn commentSpawn;
    CommentMove commentMove;
    CommentReplyArea commentReplyArea;
    
    Button thisComment;

    GameObject PowerBuffImage;
    GameObject AgilityBuffImage;

    private void Start()
    {
        playerBuffManager=GameObject.FindGameObjectWithTag("PlayerBuffManager").GetComponent<PlayerBuffManager>();
        playerStatus=GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        commentReplyArea = GameObject.FindGameObjectWithTag("CommentReplyArea").GetComponent<CommentReplyArea>();
        thisComment =this.gameObject.GetComponent<Button>();
        commentMove=this.gameObject.GetComponent<CommentMove>();

        animator = this.gameObject.GetComponent<Animator>();

        rectTransform=this.GetComponent<RectTransform>();
        PowerBuffImage = GameObject.FindGameObjectWithTag("PowerBuffImage");
        AgilityBuffImage = GameObject.FindGameObjectWithTag("AgilityBuffImage");

        trailEffect.SetActive(false);
        Pressed = false;
        commentSpawn.cheeringCommentIsExist=true;

        SetCommentAction();
    }

    void Update()
    {
        if (IsInsideReplyArea() && !Pressed)
        {
            RegisterReplyList();
        }
        else
        {
            UnregisterReplyList();
            if (commentReplyArea.canReplyComment.Count == 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    bool IsInsideReplyArea()    //返信エリアにいるか判定する関数
    {
        Canvas canvas = commentReplyArea.GetComponentInParent<Canvas>();
        Camera cam = canvas.worldCamera;

        Vector3[] commentCorners = new Vector3[4];
        rectTransform.GetWorldCorners(commentCorners);

        Vector3[] areaCorners = new Vector3[4];
        commentReplyArea.replyAreaRect.GetWorldCorners(areaCorners);

        foreach (Vector3 corner in commentCorners)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, corner);
            if (RectTransformUtility.RectangleContainsScreenPoint(commentReplyArea.replyAreaRect, screenPoint, cam))
            {
                return true;  // 1つでも入っていたらOK
            }
        }

        Vector2 commentCenter = RectTransformUtility.WorldToScreenPoint(cam, rectTransform.position);
        if (RectTransformUtility.RectangleContainsScreenPoint(commentReplyArea.replyAreaRect, commentCenter, cam))
        {
            return true;
        }

        return false;
    }

    void RegisterReplyList()    //返信可能なコメントのリストへ登録する関数
    {
        if (!commentReplyArea.canReplyComment.Contains(this.gameObject))
        {
            commentReplyArea.canReplyComment.Add(this.gameObject);
        }
    }

    void UnregisterReplyList()      //返信可能なコメントのリストから削除する関数
    {
        commentReplyArea.canReplyComment.Remove(this.gameObject);
    }

    private void SetCommentAction()
    {
        thisComment.onClick.RemoveAllListeners(); // 念のためリスナーをクリア


        if (playerStatus.Hp / playerStatus.MaxHp <= hpPercent/100)  // HPが指定%以下なら回復
        {
            thisComment.onClick.AddListener(AddHP);
        }
        else
        {
            //  HPが指定%以上ならバフ処理
            bool attackActive = playerBuffManager.attackBuff != null && playerBuffManager.attackBuff.isActive;
            bool speedActive = playerBuffManager.speedBuff != null && playerBuffManager.speedBuff.isActive;

            if (attackActive && !speedActive)
            {
                // 攻撃バフ中ならスピードバフ
                thisComment.onClick.AddListener(BuffAgility);
            }
            else if (!attackActive && speedActive)
            {
                // スピードバフ中なら攻撃バフ
                thisComment.onClick.AddListener(BuffAtack);
            }
            else
            {
                // 両方かかっている or 両方かかっていない場合はランダム
                if (Random.value < 0.5f)
                    thisComment.onClick.AddListener(BuffAtack);
                else
                    thisComment.onClick.AddListener(BuffAgility);
            }
        }
    }

    public void AddHP()
    {
        playerBuffManager.AddHP(addHPnum);
        DestroyComment();
    }

    public void BuffAtack()
    {
        AnimationBeforeMethod();

        // コルーチン完了後にBuffAttackを実行するコールバックを渡す
        OnButtonClicked(PowerBuffImage, () =>
        {
            // アニメーション完了後に実行される
            playerBuffManager.BuffAttack(attackMagnification, buffTime);
            DestroyComment();
        });
    }

    public void BuffAgility()
    {
        AnimationBeforeMethod();

        OnButtonClicked(AgilityBuffImage, () =>
        {
            playerBuffManager.BuffMoveSpeed(agilityMagnification, buffTime);
            DestroyComment();
        });
    }

    void AnimationBeforeMethod()
    {
        Pressed = true;
        pressEffect.Play();
        UnregisterReplyList();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;
        if (trailFlag)
        {
            trailEffect.SetActive(true);
        }
    }

    void DestroyComment()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        commentSpawn.cheeringCommentIsExist = false;
    }
}
