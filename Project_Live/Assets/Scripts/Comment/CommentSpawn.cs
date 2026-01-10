using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using Live2D.Cubism.Core.Unmanaged;
using UnityEngine.Rendering.Universal;


//作成者　寺村

public class CommentSpawn : MonoBehaviour
{
    [Header("応援コメントが流れるまでのコメント数")]
    [SerializeField] int cheeringCommentCount = 50;
    [Header("アンチコメントが流れるまでのコメント数")]
    [SerializeField] int antiCommentCount = 20;
    [Header("おねだりコメントが流れるまでのコメント数")]
    [SerializeField] int requestCommentCount = 20;
    [Header("返信コメントが流れるまでのコメント数")]
    [SerializeField] int replyCommnetCount = 10;
    [Header("妨害が始まったときの警告音")]
    [SerializeField] AudioClip interceptSound;
    [Header("妨害が始まったときのエフェクトの強さ")]
    [SerializeField] float effectIntensity = 20;
    [Header("妨害が始まったときのエフェクトの時間")]
    [SerializeField] float effectTime = 2;

    [Header("必要なコンポーネント")]
    public GameObject Canvas;
    [SerializeField] GameObject CommentPrefab;
    [SerializeField] GameObject CheeringCommentPrefab;
    [SerializeField] GameObject AntiCommentPrefab;
    [SerializeField] GameObject RequestCommentPrefab;
    [SerializeField] GameObject ReplyCommentPrefab;
    [SerializeField] BuzuriRank buzuriRank;
    [SerializeField] RequestManager requestManager;
    [SerializeField] AudioSource SE;
    [SerializeField] Volume volume;
    
    [HideInInspector] public bool cheeringCommentIsExist ;
    [HideInInspector] public bool antiCommentIsExist;
    [HideInInspector] public bool interceptEnemyIsExist;
    [HideInInspector] public float interceptEnemyCount;     //妨害敵用のカウンター　＊あとで敵スポーン系スクリプトのほうに統合したい

    CheeringComment cheeringComment;
    AntiComment antiComment;
    RequestCommentIdentifier requestComment;
    ReplyComment replyComment;
    //RequestManager requestManager;
    float spawnTime;
    int raneNum;
    int beforeRaneNum;
    bool first = true;
    float cheeringCommentCounter;
    float antiCommentCounter;
    float requestCommentCounter;
    float replyCommentCounter;
    RectTransform canvasRect;
    string nextText = null; //関連コメントの内容保存用string
    bool conectComment = false; //関連コメントのフラグ

    bool playSound;

    // Start is called before the first frame update
    void Start()
    {
        cheeringComment=CheeringCommentPrefab.GetComponent<CheeringComment>();
        antiComment=AntiCommentPrefab.GetComponent<AntiComment>();
        replyComment=ReplyCommentPrefab.GetComponent<ReplyComment>();
        //requestManager=GameObject.FindGameObjectWithTag("RequestManager").GetComponent<RequestManager>();
        canvasRect=Canvas.GetComponent<RectTransform>();
        cheeringCommentIsExist = false;
        antiCommentIsExist = false;
        interceptEnemyIsExist = false;
        interceptEnemyCount = 0;
        cheeringCommentCounter = 0;
        antiCommentCounter = 0;
        requestCommentCounter = 0;
        replyCommentCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        if(spawnTime>=buzuriRank.currentBuzzRank.CommentSpawnTime)
        {
            if (first)
            { 
                raneNum = Random.Range(0, 8);
                first = false;
            }
            else
            {
                while(beforeRaneNum==raneNum)
                {
                    raneNum = Random.Range(0, 8);
                }
            }

            if (cheeringCommentCounter >= cheeringCommentCount&&!cheeringCommentIsExist&&!interceptEnemyIsExist)
            {
                //Debug.Log("応援コメントが生成されました。");
                InstantiateComment(raneNum,CheeringCommentPrefab,ref nextText,ref conectComment);
                cheeringCommentCounter = 0;
            }
            else if(antiCommentCounter>=antiCommentCount/*&&!antiCommentIsExist*/)
            {
                //Debug.Log("アンチコメントが生成されました。");
                InstantiateComment(raneNum, AntiCommentPrefab,ref nextText, ref conectComment);
                antiCommentCounter = 0;
            }
            else if(requestCommentCounter>=requestCommentCount&&!interceptEnemyIsExist&&!requestManager.allRequestsIsReceipt)
            {
                Debug.Log("おねだりコメントが生成されました。");
                InstantiateComment(raneNum, RequestCommentPrefab, ref nextText, ref conectComment);
                requestCommentCounter = 0;
            }
            else if(replyCommentCounter>=replyCommnetCount&&!interceptEnemyIsExist)
            {
                InstantiateComment(raneNum, ReplyCommentPrefab,ref nextText,ref conectComment) ;
                replyCommentCounter = 0;
            }
            else
            {
                InstantiateComment(raneNum,CommentPrefab,ref nextText,ref conectComment);
                antiCommentCounter++;
                cheeringCommentCounter++;
                requestCommentCounter++;
                replyCommentCounter++;
                Debug.Log("アンチコメントまであと" + (antiCommentCount - antiCommentCounter) + "コメントです。");
                Debug.Log("応援コメントまであと" + (cheeringCommentCount - cheeringCommentCounter) + "コメントです。");
                Debug.Log("おねだりコメントまであと" + (requestCommentCount - requestCommentCounter) + "コメントです。");
            }
            
            beforeRaneNum = raneNum;
            spawnTime = 0;
        }

        if(interceptEnemyIsExist&&!playSound)
        {
            SE.PlayOneShot(interceptSound);
            StartCoroutine(InterceptEffect(effectIntensity, effectTime));
            playSound = true;
        }

        if(!interceptEnemyIsExist&&playSound)
        {
            playSound = false;
        }
    }

    //Vector2 DecideSpawnPos(RectTransform rectTransform, int raneNum)
    //{
    //    int[] rane = new int[8];
    //    for (int i = 0; i < 8; i++)
    //    {
    //        rane[i] = (int)((Canvas.rect.height - rectTransform.sizeDelta.y) / 8) * i + 1;
    //    }

    //    Vector2 screenPos = new Vector2(Canvas.rect.width, rane[raneNum]);
    //    Vector2 localPos;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas, screenPos, Camera.main, out localPos);


    //    return localPos;
    //}

    public Vector2 DecideSpawnPos(RectTransform rectTransform ,int raneNum)
    {
        float raneHeight=(canvasRect.rect.height-rectTransform.sizeDelta.y)/8;

        Vector2 CanvasMinY = new Vector2 (canvasRect.rect.width/2,-canvasRect.rect.height/2);

        Vector2 SpanwPos = new Vector2 (CanvasMinY.x,CanvasMinY.y+(raneHeight*raneNum)+rectTransform.sizeDelta.y/2);


        return SpanwPos;
    }


    void InstantiateComment(int raneNum,GameObject CommentType,ref string nextText,ref bool conectComment)
    {
        string selectedText = null;

        if (CommentType == CommentPrefab)
        {
            if(conectComment&&nextText!=null)
            {
                selectedText= nextText;

                //conectComment= false;
            }
            //selectedText = buzuriRank.currentBuzzRank.CommentContent[Random.Range(0, buzuriRank.currentBuzzRank.comentNum)];
            else
            {
                int selectedCommentNum = Random.Range(0, buzuriRank.currentBuzzRank.commentContents.Count);
                selectedText = buzuriRank.currentBuzzRank.commentContents[selectedCommentNum].MainComment;
                
                if (buzuriRank.currentBuzzRank.commentContents[selectedCommentNum].ConectComment.Count>0)
                {
                    nextText = buzuriRank.currentBuzzRank.commentContents[selectedCommentNum].ConectComment
                                            [Random.Range(0, buzuriRank.currentBuzzRank.commentContents[selectedCommentNum].ConectComment.Count)];
                    conectComment = true;
                }
            }
        }
        else if (CommentType == CheeringCommentPrefab)
        {
            int decideIndex = Random.Range(0, cheeringComment.commentContents.Count);
            selectedText = cheeringComment.commentContents[decideIndex].commentText;
        }
        else if(CommentType==AntiCommentPrefab)
        {
            int decideIndex = Random.Range(0, antiComment.commentContents.Count);
            selectedText = antiComment.commentContents[decideIndex].commentText;
        }
        else if(CommentType==ReplyCommentPrefab)
        {
            //int decideIndex = Random.Range(0, replyComment.commentContents.Count);
            int decideIndex = Random.Range(0, replyComment.rankComments[buzuriRank.CurrentIndex].commentContents.Count);
            selectedText = replyComment.rankComments[buzuriRank.CurrentIndex].commentContents[decideIndex].commentText;
        }
        
        GameObject newTextObj = Instantiate(CommentType, canvasRect);

        if (CommentType == RequestCommentPrefab)
        {
            if (newTextObj == null)
                return;
            requestComment = newTextObj.gameObject.GetComponent<RequestCommentIdentifier>();
            if (requestComment.thisRequest == null)
                Debug.LogWarning("thisRequestが設定されていません");
            selectedText = requestComment.thisRequest.commentText[Random.Range(0, requestComment.thisRequest.commentText.Count)];
        }


            GetCommetText commentText = newTextObj.GetComponent<GetCommetText>();
            RectTransform rectTransform = newTextObj.GetComponent<RectTransform>();

            // テキストを設定
            commentText.SetCommentText(selectedText);

            if (conectComment && selectedText == nextText)
            {
                if (buzuriRank.changeConectCommentCol)
                {
                    commentText.SetCommentTextColor(buzuriRank.currentBuzzRank.conectCommentColor);
                }
                conectComment = false;
            }

            rectTransform.sizeDelta = new Vector2(commentText.GetTextBoxSizeWidth(), commentText.GetTextBoxSizeHeight());
            rectTransform.anchoredPosition = DecideSpawnPos(rectTransform, raneNum);
    }

    IEnumerator InterceptEffect(float intensity,float time)
    {
        if(volume.profile.TryGet<ScreenSpaceLensFlare>(out var lensFlare))
        {
            lensFlare.intensity.value = intensity;
        }

        yield return new WaitForSeconds(time);

        lensFlare.intensity.value = 0f;
    }


    public void ChangeCheeringCommentInterval(float percent)
    {
        float changeValue = cheeringCommentCount * ((100 - percent) / 100);
        cheeringCommentCount = (int)changeValue;
        Debug.Log("応援コメントの湧き間隔が" + percent + "%減少し、湧き間隔が" + cheeringCommentCount + "になりました。");
    }

    public void ChangeAntiCommentInterval(float percent)
    {
        float changeValue = antiCommentCount + ((100 - percent) / 100);
        antiCommentCount = (int)changeValue;
        Debug.Log("アンチコメントの湧き間隔が" + percent + "%減少し、湧き間隔が" + antiCommentCount + "になりました。");
    }
}
