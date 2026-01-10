using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;


//制作者　寺村

public class RequestCommentIdentifier :ReplyCommentBase
{
    [Header("敵おねだり一覧")]
    [SerializeField] List<RequestEnemySO> requestEnemySOs;
    [Header("バズリショットおねだり一覧")]
    [SerializeField] List<RequestBazuriShotSO> requestBazuriShotSOs;
    [Header("エモートおねだり一覧")]
    [SerializeField] List<RequestEmoteSO> requestEmoteSOs;
    [Header("ボイス一覧")]
    [SerializeField]List<AudioClip> replyVoices;


    GameObject targetObj;
    RequestManager requestManager;

    public RequestBaseSO thisRequest { private set; get; }
    bool receipt;

    //List<RequestBaseSO> allRequests = new List<RequestBaseSO>();

    void Awake()
    {
        InitializeAnything();

        requestManager = GameObject.FindGameObjectWithTag("RequestManager").GetComponent<RequestManager>();
        targetObj = GameObject.FindGameObjectWithTag("Denkou");

        if (!InitializeRequest())
        {
            Destroy(this.gameObject);
        }

        if (thisRequest == null)
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

        if (replyVoices.Count > 0)
        {
            decideVoice = replyVoices[Random.Range(0, replyVoices.Count)];
        }

        receipt = false;

        thisButton.onClick.AddListener(ReceiptRequest);
    }

    private void Start()
    {
        //InitializeAnything();

        //requestManager = GameObject.FindGameObjectWithTag("RequestManager").GetComponent<RequestManager>();
        //targetObj = GameObject.FindGameObjectWithTag("Denkou");

        //StartCoroutine(InitializeRequestCoroutine());  // コルーチン開始

        //if (!InitializeRequest())
        //{
        //    Destroy(this.gameObject);
        //}

        //if (thisRequest == null)
        //{
        //    Debug.Log("このコメントにはリクエストが格納されていません");
        //}

        //switch (thisRequest.requestType)
        //{
        //    case RequestBaseSO.RequestType.Enemy:
        //        requestManager.requestEnemyIsExist = true;
        //        break;
        //    case RequestBaseSO.RequestType.Emote:
        //        requestManager.requestEmoteIsExist = true;
        //        break;
        //    case RequestBaseSO.RequestType.BazuriShot:
        //        requestManager.requestBazuriShotIsExist = true;
        //        break;
        //}

        //if (replyVoices.Count > 0)
        //{
        //    decideVoice = replyVoices[Random.Range(0, replyVoices.Count)];
        //}

        //receipt = false;

        //thisButton.onClick.AddListener(ReceiptRequest);

        //if (thisRequest == null)
        //    Debug.LogWarning("thisRequestが設定されていません");
        //string selectedText =thisRequest.commentText[Random.Range(0,thisRequest.commentText.Count)];

        //GetCommetText commentText = this.GetComponent<GetCommetText>();
        //RectTransform rectTransform = this.GetComponent<RectTransform>();

        //// テキストを設定
        //commentText.SetCommentText(selectedText);

        //rectTransform.sizeDelta = new Vector2(commentText.GetTextBoxSizeWidth(), commentText.GetTextBoxSizeHeight());
        //rectTransform.anchoredPosition = commentSpawn.DecideSpawnPos(rectTransform, commentSpawn.raneNum);

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }

    public bool InitializeRequest()
    {
        // ここでランダムで一つ選ぶ例
        var allRequests = new List<RequestBaseSO>();
        if (!requestManager.requestEnemyIsExist && !requestManager.requestEnemyIsReceipt)
        {
            allRequests.AddRange(requestEnemySOs);
           // LoadRequestAsync("EnemyRequest");
            //allRequests.Add(enemyReq);
        }
        if (!requestManager.requestBazuriShotIsExist && !requestManager.requestBazuriShotIsReceipt)
        {
            allRequests.AddRange(requestBazuriShotSOs);
            //LoadRequestAsync("ShotRequest");
            //allRequests.Add(shotReq);
        }

        if (!requestManager.requestEmoteIsExist && !requestManager.requestEmoteIsReceipt)
        {
            allRequests.AddRange(requestEmoteSOs);
            //LoadRequestAsync("EmoteRequest");
            //allRequests.Add(emoteReq);
        }
        if (allRequests.Count > 0)
        {
            thisRequest = allRequests[Random.Range(0, allRequests.Count)];
            //allRequests.Clear();
            return true;
        }

        return false;
    }

    public void ReceiptRequest()
    {
        AnimationBeforeMethod();
        PlaySound();
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
                    requestManager.currentRequestEmote = (RequestEmoteSO)thisRequest;
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

    //public async void LoadRequestAsync(string address)
    //{
    //    var handle = Addressables.LoadAssetAsync<RequestBaseSO>("Requests/"+address);
    //    await handle.Task;

    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        var req = handle.Result;
    //        allRequests.Add(req);
    //        Debug.Log("Addressables Load Success: " + req.displayText);
    //    }
    //    else
    //    {
    //        Debug.LogError("Addressables Load Failed: " + address);
    //    }
    //}

    //public IEnumerator InitializeRequest()  // IEnumerator に変更
    //{
    //    if (!requestManager.requestEnemyIsExist && !requestManager.requestEnemyIsReceipt)
    //    {
    //        yield return StartCoroutine(LoadRequestCoroutine("EnemyRequest"));
    //    }
    //    if (!requestManager.requestBazuriShotIsExist && !requestManager.requestBazuriShotIsReceipt)
    //    {
    //        yield return StartCoroutine(LoadRequestCoroutine("ShotRequest"));
    //    }
    //    if (!requestManager.requestEmoteIsExist && !requestManager.requestEmoteIsReceipt)
    //    {
    //        yield return StartCoroutine(LoadRequestCoroutine("EmoteRequest"));
    //    }

    //    if (allRequests.Count > 0)
    //    {
    //        thisRequest = allRequests[Random.Range(0, allRequests.Count)];
    //        allRequests.Clear();
    //        yield return true;  // 成功
    //    }
    //    else
    //    {
    //        yield return false;
    //    }
    //}

    // コルーチン版LoadRequest
    //private IEnumerator LoadRequestCoroutine(string address)
    //{
    //    var handle = Addressables.LoadAssetAsync<RequestBaseSO>("Requests/"+address);  // "Requests/"+address を削除
    //    yield return handle;

    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        var req = handle.Result;
    //        allRequests.Add(req);
    //        Debug.Log("Load Success: " + req.displayText);
    //    }
    //    else
    //    {
    //        Debug.LogError("Load Failed: " + address);
    //    }
    //    Addressables.Release(handle);  // 重要：メモリ解放
    //}

    //private IEnumerator InitializeRequestCoroutine()
    //{
    //    bool success = false;
    //    yield return StartCoroutine(InitializeRequest());  // ロード待機
    //    success = (thisRequest != null);

    //    if (!success)
    //    {
    //        Destroy(this.gameObject);
    //        yield break;
    //    }

    //    if (thisRequest == null)
    //    {
    //        Debug.Log("このコメントにはリクエストが格納されていません");
    //    }

    //    switch (thisRequest.requestType)
    //    {
    //        case RequestBaseSO.RequestType.Enemy:
    //            requestManager.requestEnemyIsExist = true;
    //            break;
    //        case RequestBaseSO.RequestType.Emote:
    //            requestManager.requestEmoteIsExist = true;
    //            break;
    //        case RequestBaseSO.RequestType.BazuriShot:
    //            requestManager.requestBazuriShotIsExist = true;
    //            break;
    //    }

    //    if (replyVoices.Count > 0)
    //    {
    //        decideVoice = replyVoices[Random.Range(0, replyVoices.Count)];
    //    }

    //    receipt = false;

    //    thisButton.onClick.AddListener(ReceiptRequest);

    //    if (thisRequest == null)
    //        Debug.LogWarning("thisRequestが設定されていません");
    //    string selectedText = thisRequest.commentText[Random.Range(0, thisRequest.commentText.Count)];

    //    GetCommetText commentText = this.GetComponent<GetCommetText>();
    //    RectTransform rectTransform = this.GetComponent<RectTransform>();

    //    // テキストを設定
    //    commentText.SetCommentText(selectedText);

    //    rectTransform.sizeDelta = new Vector2(commentText.GetTextBoxSizeWidth(), commentText.GetTextBoxSizeHeight());
    //    rectTransform.anchoredPosition = commentSpawn.DecideSpawnPos(rectTransform, commentSpawn.raneNum);

    //    BoxCollider2D collider = GetComponent<BoxCollider2D>();
    //    collider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    //}


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
