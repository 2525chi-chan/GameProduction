using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

public class ReplyComment : ReplyCommentBase
{
    [Header("返信した時にもらえるいいね数")]
    [SerializeField] List<int> GetGoodNum;

    int getGoodNum;

    GoodSystem goodSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeAnything();

        if (GetGoodNum.Count > 0)
        {
            getGoodNum = GetGoodNum[UnityEngine.Random.Range(0, GetGoodNum.Count)];
        }

        goodSystem = GameObject.FindGameObjectWithTag("GoodSystem").GetComponent<GoodSystem>();
        thisButton.onClick.RemoveAllListeners();
        thisButton.onClick.AddListener(PressMethod);
    }

    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(2.0f);

        goodSystem.AddGood(getGoodNum);
        
        Destroy(this.gameObject);
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

        StartCoroutine(WaitSecond());
    }
}
