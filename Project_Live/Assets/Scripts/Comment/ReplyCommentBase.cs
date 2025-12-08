using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//制作者　寺村
//返信コメント系のベースクラス
//返信コメント系は必ずこのクラスの派生クラスにする

public abstract class ReplyCommentBase : MonoBehaviour
{
    [Header("コメントの内容")]
    public List<string> commentContents = new List<string>();

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

        if (trailEffect != null)
            trailEffect.SetActive(false);
        Pressed = false;
        isAnimating = false;
    }

    protected void CheckInArea()    //＊返信コメント系のUpdate関数で必ず呼ぶ、エリア内にいるか判定し、返信できるListに登録する関数
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

    protected bool IsInsideReplyArea()    //返信エリアにいるか判定する関数
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
        yield return new WaitForSeconds(0.5f);

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
}
