using System.Collections.Generic;
using UnityEngine;
using TMPro;

//制作者　寺村

public class RequestCommentIdentifier :ReplyCommentBase
{
    [Header("敵おねだり一覧")]
    [SerializeField] List<RequestEnemySO> requestEnemySOs;
    [Header("バズリショットおねだり一覧")]
    [SerializeField] List<RequestBazuriShotSO> requestBazuriShotSOs;
    [Header("エモートおねだり一覧")]
    [SerializeField] List<RequestEmoteSO> requestEmoteSOs;


    GameObject targetObj;
    RequestManager requestManager;

    public RequestBaseSO thisRequest { private set; get; }
    bool receipt;

    void Awake()
    {
        InitializeAnything();

        requestManager=GameObject.FindGameObjectWithTag("RequestManager").GetComponent<RequestManager>();
        targetObj= GameObject.FindGameObjectWithTag("Denkou");

        if (!InitializeRequest())
        {
            Destroy(this.gameObject);
        }

        if(thisRequest==null)
        {
            Debug.Log("このコメントにはリクエストが格納されていません");
        }

        switch (thisRequest.requestType)
        {
            case RequestBaseSO.RequestType.Enemy:
                requestManager.requestEnemyIsExist = true;
                break;
            case RequestBaseSO.RequestType.Emote:
                requestManager.requestEmoteIsExist = true;
                break;
            case RequestBaseSO.RequestType.BazuriShot:
                requestManager.requestBazuriShotIsExist = true;
                break;
        }

        receipt = false;

        thisButton.onClick.AddListener(ReceiptRequest);
    }

    void Update()
    {
        CheckInArea();
    }

    public bool InitializeRequest()
    {
        // ここでランダムで一つ選ぶ例
        var allRequests = new List<RequestBaseSO>();
        if(!requestManager.requestEnemyIsExist&&!requestManager.requestEnemyIsReceipt)
        allRequests.AddRange(requestEnemySOs);
        if(!requestManager.requestBazuriShotIsExist&&!requestManager.requestBazuriShotIsReceipt)
        allRequests.AddRange(requestBazuriShotSOs);
        if(!requestManager.requestEmoteIsExist&&!requestManager.requestEmoteIsReceipt)
        allRequests.AddRange(requestEmoteSOs);

        if (allRequests.Count > 0)
        {
            thisRequest = allRequests[Random.Range(0, allRequests.Count)];
            return true;
        }

        return false;
    }

    public void ReceiptRequest()
    {
        AnimationBeforeMethod();
        requestManager.addRequest = true;
        OnButtonClickedAnimation(targetObj, () =>
        {

            requestManager.RegistRequest(thisRequest);
            switch (thisRequest.requestType)
            {
                case RequestBaseSO.RequestType.Enemy:
                    requestManager.currentRequestEnemy = (RequestEnemySO)thisRequest;
                    requestManager.requestEnemyIsReceipt = true;
                    break;
                case RequestBaseSO.RequestType.Emote:
                    requestManager.requestEmoteIsReceipt = true;
                    break;
                case RequestBaseSO.RequestType.BazuriShot:
                    requestManager.currentRequestBazuriShot = (RequestBazuriShotSO)thisRequest;
                    requestManager.requestBazuriShotIsReceipt = true;
                    break;
            }
            requestManager.DisplayRequestText(thisRequest);
            receipt = true;
            requestManager.addRequest = false;
            Destroy(this.gameObject);
        });
    }

    void OnDestroy()
    {
        if (!receipt)
        {
            switch (thisRequest.requestType)
            {
                case RequestBaseSO.RequestType.Enemy:
                    requestManager.requestEnemyIsExist = false;
                    break;
                case RequestBaseSO.RequestType.Emote:
                    requestManager.requestEmoteIsExist = false;
                    break;
                case RequestBaseSO.RequestType.BazuriShot:
                    requestManager.requestBazuriShotIsExist = false;
                    break;
            }
        }
    }
}
