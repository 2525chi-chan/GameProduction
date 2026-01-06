//　制作者　寺村

using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AntiComment : ReplyCommentBase
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

    GoodSystem goodSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //返信コメント共通の準備部分
        InitializeAnything();

        goodSystem = GameObject.FindGameObjectWithTag("GoodSystem").GetComponent<GoodSystem>();
        thisButton.onClick.RemoveAllListeners();
        thisButton.onClick.AddListener(DecreaseGoodNum);
        commentSpawn.antiCommentIsExist = true;
    }

    void Update()
    {
        //返信コメント共通のエリア判定部
        CheckInArea();

        if (Pressed && !goodSystem.Decreasing)  //いいねの減少が終わってからこのコメントを削除する
        {
            Destroy(this.gameObject);
        }
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
    }

    void PressMethod()  //押された瞬間のエフェクトやコメント移動の停止関数
    {
        Pressed = true;
        pressEffect.Play();
        animator.Play("CommentHighlight");
        PlaySound();
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
