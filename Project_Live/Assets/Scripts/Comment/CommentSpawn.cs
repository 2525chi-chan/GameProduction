using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using JetBrains.Annotations;


//作成者　寺村

public class CommentSpawn : MonoBehaviour
{
    //[Header("コメントの種類")]
    //[SerializeField] List<string> commentContent=new List<string>();

    [Header("応援コメントが流れるまでのコメント数")]
    [SerializeField] float commentCount = 50;

    [Header("必要なコンポーネント")]
    [SerializeField] Transform Canvas;
    [SerializeField] GameObject CommentPrefab;
    [SerializeField] GameObject CheeringCommentPrefab;
    [SerializeField] BuzuriRank buzuriRank;

    [HideInInspector]public bool cheeringCommentIsExist ;

    CheeringComment cheeringComment;
    float spawnTime;
    int raneNum;
    int beforeRaneNum;
    bool first = true;
    float commentCounter;

    // Start is called before the first frame update
    void Start()
    {
        cheeringComment=CheeringCommentPrefab.GetComponent<CheeringComment>();
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

            if (commentCounter >= commentCount&&!cheeringCommentIsExist)
            {
                Debug.Log("応援コメントが生成されました。");
                InstantiateComment(raneNum,CheeringCommentPrefab);
                commentCounter = 0;
            }
            else
            {
                InstantiateComment(raneNum,CommentPrefab);
                commentCounter++;
                Debug.Log("応援コメントまであと" + (commentCount - commentCounter) + "コメントです。");
            }
            
            beforeRaneNum = raneNum;
            spawnTime = 0;
        }
    }

    Vector2 DecideSpawnPos(RectTransform rectTransform,int raneNum)
    {
        int[] rane = new int[8]; 
        for (int i=0;i<8;i++)
        {
            rane[i] = (int)((Screen.height - rectTransform.sizeDelta.y) / 8) * i + 1;
        }

        Vector2 spawnPos = new Vector2(Screen.width, rane[raneNum]);

        return spawnPos;
    }


    void InstantiateComment(int raneNum, GameObject CommentType)
    {
        string selectedText = null;

        if (CommentType == CommentPrefab)
        {
            selectedText = buzuriRank.currentBuzzRank.CommentContent[Random.Range(0, buzuriRank.currentBuzzRank.comentNum)];
        }
        else if (CommentType == CheeringCommentPrefab)
        {
            selectedText = cheeringComment.cheeringCommentContent[Random.Range(0, cheeringComment.cheeringCommentContent.Count)];
        }


        GameObject newTextObj = Instantiate(CommentType, Canvas);

        if(CommentType==CheeringCommentPrefab)
        {
            EventSystem.current.SetSelectedGameObject(newTextObj);
        }

        GetCommetText commentText = newTextObj.GetComponent<GetCommetText>();
        RectTransform rectTransform = newTextObj.GetComponent<RectTransform>();

        // テキストを設定
        commentText.SetCommentText(selectedText);

        rectTransform.sizeDelta = new Vector2(commentText.GetTextBoxSizeWidth(), rectTransform.sizeDelta.y);
        rectTransform.position = DecideSpawnPos(rectTransform, raneNum);
    }
}
