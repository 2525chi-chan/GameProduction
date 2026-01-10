using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using TMPro;
using System.Net;
using static UnityEngine.UI.GridLayoutGroup;
using Unity.VisualScripting;

//制作者　寺村
//返信コメント系のベースクラス
//返信コメント系は必ずこのクラスの派生クラスにする



public abstract class ReplyCommentBase : MonoBehaviour
{
    [System.Serializable]
    public class CommentContents
    {
        [Header("コメントのテキスト内容")]
        public string commentText;
        [Header("このコメントでラマユウがしゃべるボイス")]
        public AudioClip replyVoice;
    }
    [System.Serializable]
    public class RankComments
    {
        [Header("バズリランク名")]
        [SerializeField] string name;
        [Header("このバズリランクで生成するコメントの内容")]
        public List<CommentContents> commentContents=new List<CommentContents>();
    }


    protected AudioClip decideVoice;    //生成されたときにテキストに合わせたボイスが格納される

    //[Header("コメントの内容")]
    //public List<string> commentContents = new List<string>();

    [Header("コメントの内容")]
    public List<CommentContents> commentContents = new List<CommentContents>();

    [Header("各バズリランクで生成するコメント")]
    public List<RankComments>　rankComments = new List<RankComments>();

    [Header("決定時のエフェクト")]
    [SerializeField] protected ParticleSystem pressEffect;
    [Header("軌跡を描くエフェクト（＊アニメーションしない場合はNone")]
    [SerializeField] protected GameObject trailEffect;
    [Header("軌跡エフェクトの有無")]
    [SerializeField] protected bool trailFlag;

    [Header("アニメーション秒数")]
    [SerializeField] protected float animationDuration = 0.5f;
    [Header("アニメーションカーブ")]
    [SerializeField] protected AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("このコメントを受け取った時の効果音")]
    [SerializeField] protected AudioClip commentSound;

    [Header("必要なコンポーネント")]
    [SerializeField] TextMeshProUGUI text;

    protected AudioSource SE;
    protected AudioSource Main_Voice;

    protected RectTransform rectTransform;  //このオブジェクトのRectTransform
    protected RectTransform targetPosition; //オブジェクトが向かっていく位置のRectTransform
    protected bool isAnimating;
    protected bool Pressed;
    protected Animator animator;

    protected CommentSpawn commentSpawn;
    protected CommentMove commentMove;
    protected CommentReplyArea commentReplyArea;

    protected Button thisButton;

    protected void InitializeAnything() //＊返信コメント系のStart関数で必ず呼ぶ
    {
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        commentReplyArea = GameObject.FindGameObjectWithTag("CommentReplyArea").GetComponent<CommentReplyArea>();
        commentMove = this.gameObject.GetComponent<CommentMove>();

        thisButton = this.gameObject.GetComponent<Button>();
        animator = this.gameObject.GetComponent<Animator>();
        rectTransform = this.GetComponent<RectTransform>();

        SE = GameObject.FindGameObjectWithTag("SE").GetComponent<AudioSource>();
        Main_Voice = GameObject.FindGameObjectWithTag("Main_Voice").GetComponent<AudioSource>();

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        if (commentContents.Count>0)
        decideVoice = commentContents.Find(c => c.commentText == text.text).replyVoice;

        if (trailEffect != null)
            trailEffect.SetActive(false);
        Pressed = false;
        isAnimating = false;
    }

    protected void RegisterReplyList()    //返信可能なコメントのリストへ登録する関数
    {
        if (!commentReplyArea.canReplyComment.Contains(this.gameObject))
        {
            commentReplyArea.canReplyComment.Add(this.gameObject);
        }
    }

    protected void UnregisterReplyList()      //返信可能なコメントのリストから削除する関数
    {
        commentReplyArea.canReplyComment.Remove(this.gameObject);
    }

    protected void OnButtonClickedAnimation(GameObject targetObj, Action onComplete = null) //アニメーションがある場合のコメント取得関数
    {
        // ターゲット位置を見つける
        if (targetPosition == null)
        {
            if (targetObj != null)
            {
                targetPosition = targetObj.GetComponent<RectTransform>();
                Debug.Log("ターゲットが設定されました");
            }
        }

        if (targetPosition != null && !isAnimating)
        {
            //Debug.Log("コメントの移動を開始します");
            // コールバックをコルーチンに渡す
            StartCoroutine(AnimateToTarget(onComplete));
        }
        else
        {
            // アニメーションしない場合でもコールバックを実行
            onComplete?.Invoke();
        }
    }

    protected IEnumerator AnimateToTarget(Action onComplete = null) //アニメーション関数
    {

        animator.Play("CommentHighlight");

        yield return new WaitForSeconds(2.0f);

        animator.Play("ChangeScale");

        isAnimating = true;

        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = new Vector2(
            targetPosition.anchoredPosition.x ,
            targetPosition.anchoredPosition.y 
        );
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float curveValue = moveCurve.Evaluate(t);

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        isAnimating = false;

        // コールバックを実行
        onComplete?.Invoke();
    }

    protected void AnimationBeforeMethod()  //アニメーション前に呼ぶ関数
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

    protected void PlaySound()  //音系を鳴らす関数
    {
        SE.PlayOneShot(commentSound);
        //if(!Main_Voice.isPlaying)
        Main_Voice.PlayOneShot(decideVoice);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="CommentReplyArea"&&!Pressed)
        {
            RegisterReplyList();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="CommentReplyArea")
        {
            UnregisterReplyList();
            if (commentReplyArea.canReplyComment.Count == 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
