using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class RequestManager : MonoBehaviour
{
    [Header("リクエストクリア時のスタンプ音")]
    [SerializeField] AudioClip clearStamp;

    [Header("必要なコンポーネント")]
    [SerializeField] GameObject denkou;
    [SerializeField] GameObject requestText;
    [SerializeField] Canvas InGameCanvas;
    [SerializeField] GoodSystem goodSystem;
    [SerializeField] CommentSpawn commentSpawn;
    [SerializeField] TextMeshProUGUI ClearText;
    [SerializeField] GameObject InterceptText;
    [SerializeField] GameObject ClearTextAnimation;
    [SerializeField] GameObject ClearStampAnimation;
    [SerializeField] AudioSource SE;

    [System.NonSerialized] public List<RequestBaseSO> currentRequests;
    [System.NonSerialized] public bool requestEnemyIsExist;
    [System.NonSerialized] public bool requestEnemyIsReceipt;
    [System.NonSerialized] public bool requestEmoteIsExist;
    [System.NonSerialized] public bool requestEmoteIsReceipt;
    [System.NonSerialized] public bool requestBazuriShotIsExist;
    [System.NonSerialized] public bool requestBazuriShotIsReceipt;
    [System.NonSerialized] public bool allRequestsIsReceipt;
    [System.NonSerialized] public bool isIntercepting;

    [System.NonSerialized] public bool denkouDisplay;
    [System.NonSerialized] public bool addRequest;
    [System.NonSerialized] public bool isAnimating;

    [System.NonSerialized] public RequestBazuriShotSO currentRequestBazuriShot;
    [System.NonSerialized] public RequestEnemySO currentRequestEnemy;
    [System.NonSerialized] public RequestEmoteSO currentRequestEmote;

    
    Animator ClearTextAnimator;
    Animator ClearStampAnimator;
    int nextDisplayIndex;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRequests = new List<RequestBaseSO>();
        nextDisplayIndex = 0;
        requestEnemyIsExist=false;
        requestEnemyIsReceipt=false;
        requestEmoteIsExist=false;
        requestEmoteIsReceipt=false;
        requestBazuriShotIsExist = false;
        requestBazuriShotIsReceipt=false;
        allRequestsIsReceipt = false;
        isIntercepting = false;
        denkouDisplay = false;
        addRequest = false;
        isAnimating = false;

        InterceptText.SetActive(false);
        
        ClearTextAnimator=ClearTextAnimation.GetComponent<Animator>();
        ClearTextAnimation.SetActive(false);
        ClearStampAnimator=ClearStampAnimation.GetComponent<Animator>();
        ClearStampAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeInterceptMode(commentSpawn.interceptEnemyIsExist);

        if (requestBazuriShotIsExist && requestEmoteIsExist && requestEnemyIsExist)
            allRequestsIsReceipt = true;
        else if (requestBazuriShotIsReceipt && requestEmoteIsReceipt && requestEnemyIsReceipt)
            allRequestsIsReceipt = true;
        else
            allRequestsIsReceipt = false;

        if (currentRequests.Count > 0&&!denkouDisplay&&!addRequest&&!isAnimating&&!isIntercepting)
        {
            DisplayRequestText(currentRequests[nextDisplayIndex]);
            if (nextDisplayIndex == currentRequests.Count - 1)
            {
                nextDisplayIndex = 0;
            }
            else
            {
                nextDisplayIndex++;
            }
        }

        if(isAnimating)
        {
            nextDisplayIndex = 0;
        }
    }

    public void DisplayRequestText(RequestBaseSO request)
    {
        RectTransform spawnRect=denkou.GetComponent<RectTransform>();

        GameObject newTextObj = Instantiate(requestText,denkou.transform);
        RectTransform newRect=newTextObj.GetComponent<RectTransform>();
        TextMeshProUGUI newText = newTextObj.GetComponent<TextMeshProUGUI>();
        
        newText.text = request.displayText;

        if(request.requestType==RequestBaseSO.RequestType.Enemy)
        {
            newText.GetComponent<DenkouMove>().thisIsEnemy = true;
        }

        float spawnPosX = spawnRect.rect.width+(newText.preferredWidth/2);
        
        newTextObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(spawnPosX,0);

        
    }

    public void SuccessRequest(RequestBaseSO request)
    {
        isAnimating = true;
        StartCoroutine(PlaySuccessAnimation(request));
        //goodSystem.AddGood(request.getGoodNum);
        //commentSpawn.ChangeCheeringCommentInterval(request.cheeringIncreasePercent);
        //UnregistRequest(request);
    }

    IEnumerator PlaySuccessAnimation(RequestBaseSO request)
    {
        ClearTextAnimation.SetActive(true);
        ClearText.text = request.displayText;
        ClearTextAnimator.Play("MoveCenter", 0, 0f);

        // normalizedTimeが1.0f以上になるまで待機（アニメーション終了）
        while (ClearTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        ClearStampAnimation.SetActive(true);
        ClearStampAnimator.Play("ClearStamp", 0, 0f);
        SE.PlayOneShot(clearStamp);

        while(ClearStampAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime<1.0f)
        {
            yield return null;
        }


        ClearStampAnimation.SetActive(false);
        ClearTextAnimation.SetActive(false);
        Debug.Log("アニメーション処理終了");

        isAnimating = false;

        goodSystem.AddGood(request.getGoodNum);
        commentSpawn.ChangeCheeringCommentInterval(request.cheeringIncreasePercent);
        UnregistRequest(request);
    }

    public void FailedRequest(RequestBaseSO request)
    {
        goodSystem.DecreaseGood(request.decreaseGoodNum);
        commentSpawn.ChangeAntiCommentInterval(request.antiIncreasePercent);
        UnregistRequest(request);
    }

    public void RegistRequest(RequestBaseSO request)
    {
        currentRequests.Add(request);
    }

    public void UnregistRequest(RequestBaseSO request)
    {
        
        currentRequests.Remove(request);
        
        switch (request.requestType)
        {
            case RequestBaseSO.RequestType.Enemy:
                currentRequestEnemy.enemyCounter = 0;
                currentRequestEnemy = null;
                requestEnemyIsReceipt = false;
                requestEnemyIsExist = false;
                break;
            case RequestBaseSO.RequestType.Emote:
                currentRequestEmote = null;
                requestEmoteIsReceipt = false;
                requestEmoteIsExist = false;
                break;
            case RequestBaseSO.RequestType.BazuriShot:
                currentRequestBazuriShot = null; 
                requestBazuriShotIsReceipt = false; 
                requestBazuriShotIsExist = false;
                break;
        }
    }

    void ChangeInterceptMode(bool interceptEnemyFlag)
    {
        if (interceptEnemyFlag)
        {
            isIntercepting = true;
            InterceptText.SetActive(true);
        }
        else if (!interceptEnemyFlag)
        {
            isIntercepting = false;
            InterceptText.SetActive(false);
        }
    }

    public void CheckBazuriShot(int value)
    {
        if (currentRequestBazuriShot == null)
            Debug.LogWarning("バズリショットのリクエストが格納されていません");

        if (value >= currentRequestBazuriShot.requestNum)
        {
            SuccessRequest(currentRequestBazuriShot);
            //Debug.Log("バズリショットのおねだりを達成しました。");
        }
    }

    public void SuccessEnemyRequest()
    {
        SuccessRequest(currentRequestEnemy);
    }

    public void SuccessEmoteRequest()
    {
        SuccessRequest(currentRequestEmote);
    }
}
