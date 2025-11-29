//　制作者　寺村

using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AntiComment : MonoBehaviour
{
    public enum DecreaseType
    { 
        Fixed,
        Pecentage,
        Mixed
    }

    [Header("固定値で減らすか、パーセンテージで減らすか、混合か")]
    [SerializeField] DecreaseType decreaseType;
    [Header("受け取ったら減らすいいね数(固定値)")]
    [SerializeField] int[] fixedValueDecrease;
    [Header("受け取ったら減らすいいね数(割合)")]
    [SerializeField, Range(0, 100)] int decreaseRate;
    [Header("何いいね以上で減少方法を固定値→割合に変更するか")]
    [SerializeField] int changeDecreaseTypeNum;
    [Header("アンチコメントの内容")]
    public List<string> antiCommentContent = new List<string>();
    [Header("決定時のエフェクト")]
    [SerializeField] ParticleSystem pressEffect;

    bool Pressed;
    
    RectTransform thisRect;
    Button operationDetail;
    GoodSystem goodSystem;
    CommentMove commentMove;
    CommentSpawn commentSpawn;
    CommentReplyArea commentReplyArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goodSystem = GameObject.FindGameObjectWithTag("GoodSystem").GetComponent<GoodSystem>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        commentReplyArea = GameObject.FindGameObjectWithTag("CommentReplyArea").GetComponent<CommentReplyArea>();
        commentMove=this.GetComponent<CommentMove>();
        thisRect = this.GetComponent<RectTransform>();
        operationDetail=this.GetComponent<Button>();
        operationDetail.onClick.RemoveAllListeners();
        operationDetail.onClick.AddListener(DecreaseGoodNum);
        Pressed = false;
        commentSpawn.antiCommentIsExist = true;
    }

    void Update()
    {
        if(IsInsideReplyArea()&&!Pressed)
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

        if (Pressed && !goodSystem.Decreasing)
        {
            Destroy(this.gameObject);
        }
    }

    bool IsInsideReplyArea()    //返信エリアにいるか判定する関数
    {
        Canvas canvas = commentReplyArea.GetComponentInParent<Canvas>();
        Camera cam = canvas.worldCamera;

        Vector3[] commentCorners = new Vector3[4];
        thisRect.GetWorldCorners(commentCorners);

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

        Vector2 commentCenter = RectTransformUtility.WorldToScreenPoint(cam, thisRect.position);
        if (RectTransformUtility.RectangleContainsScreenPoint(commentReplyArea.replyAreaRect, commentCenter, cam))
        {
            return true;
        }

        return false;
    }

    void RegisterReplyList()    //返信可能なコメントのリストへ登録する関数
    {
        if(!commentReplyArea.canReplyComment.Contains(this.gameObject))
        {
            commentReplyArea.canReplyComment.Add(this.gameObject);
        }
    }

    void UnregisterReplyList()      //返信可能なコメントのリストから削除する関数
    {
        commentReplyArea.canReplyComment.Remove(this.gameObject);
    }

    public void DecreaseGoodNum ()  //コメントが拾われたら行われる関数
    {
        PressMethod();

        int decreaseValue = 0;

        switch (decreaseType)
        {
            case DecreaseType.Fixed:
                decreaseValue = FixedDecrease();
                    break;
            case DecreaseType.Pecentage:
                decreaseValue = PercentageDecrease();
                    break;
            case DecreaseType.Mixed:
                decreaseValue = MixedDecrease();
                break;
        }

        StartCoroutine(goodSystem.DecreaseGood(decreaseValue));

        //goodSystem.DecreaseGood(decreaseValue);
    }

    void PressMethod()  //押された瞬間のエフェクトやコメント移動の停止関数
    {
        Pressed = true;
        pressEffect.Play();
        UnregisterReplyList();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;
    }

    int MixedDecrease() //混合減少の関数
    {
        int value;

        if(goodSystem.GoodNum>=changeDecreaseTypeNum)
        {
            return value=PercentageDecrease();
        }
        else
        {
            return value=FixedDecrease();
        }
    }

    int FixedDecrease() //固定値減少の関数
    {
        int value= fixedValueDecrease[Random.Range(0, fixedValueDecrease.Length)];
        Debug.Log("アンチコメントによるいいねの減少量は" + value + "です。");

        return value;
    }

    int PercentageDecrease()    //％減少の関数
    {
        int value= (int)(goodSystem.GoodNum * decreaseRate / 100);
        Debug.Log("減少率が" + decreaseRate + "%と設定されているため、いいね数" + goodSystem.GoodNum + "から減少量は" + value + "です。");
        
        return value;
    }

    void OnDestroy()
    {
        commentSpawn.antiCommentIsExist = false;   
    }
}
