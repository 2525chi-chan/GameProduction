using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using JetBrains.Annotations;


//�쐬�ҁ@����

public class CommentSpawn : MonoBehaviour
{
    //[Header("�R�����g�̎��")]
    //[SerializeField] List<string> commentContent=new List<string>();

    [Header("�����R�����g�������܂ł̃R�����g��")]
    [SerializeField] float commentCount = 50;

    [Header("�K�v�ȃR���|�[�l���g")]
    public GameObject Canvas;
    [SerializeField] GameObject CommentPrefab;
    [SerializeField] GameObject CheeringCommentPrefab;
    [SerializeField] BuzuriRank buzuriRank;

    [HideInInspector] public bool cheeringCommentIsExist ;
    [HideInInspector] public bool interceptEnemyIsExist;
    [HideInInspector] public float interceptEnemyCount;     //�W�Q�G�p�̃J�E���^�[�@�����ƂœG�X�|�[���n�X�N���v�g�̂ق��ɓ���������

    CheeringComment cheeringComment;
    float spawnTime;
    int raneNum;
    int beforeRaneNum;
    bool first = true;
    float commentCounter;
    RectTransform canvasRect;

    // Start is called before the first frame update
    void Start()
    {
        cheeringComment=CheeringCommentPrefab.GetComponent<CheeringComment>();
        canvasRect=Canvas.GetComponent<RectTransform>();
        interceptEnemyIsExist = false;
        interceptEnemyCount = 0;
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

            if (commentCounter >= commentCount&&!cheeringCommentIsExist&&!interceptEnemyIsExist)
            {
                Debug.Log("�����R�����g����������܂����B");
                InstantiateComment(raneNum,CheeringCommentPrefab);
                commentCounter = 0;
            }
            else
            {
                InstantiateComment(raneNum,CommentPrefab);
                commentCounter++;
                Debug.Log("�����R�����g�܂ł���" + (commentCount - commentCounter) + "�R�����g�ł��B");
            }
            
            beforeRaneNum = raneNum;
            spawnTime = 0;
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

    Vector2 DecideSpawnPos(RectTransform rectTransform ,int raneNum)
    {
        float raneHeight=(canvasRect.rect.height-rectTransform.sizeDelta.y)/8;

        Vector2 CanvasMinY = new Vector2 (canvasRect.rect.width/2,-canvasRect.rect.height/2);

        Vector2 SpanwPos = new Vector2 (CanvasMinY.x,CanvasMinY.y+(raneHeight*raneNum)+rectTransform.sizeDelta.y/2);


        return SpanwPos;
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


        GameObject newTextObj = Instantiate(CommentType, canvasRect);

        if(CommentType==CheeringCommentPrefab)
        {
            EventSystem.current.SetSelectedGameObject(newTextObj);
        }

        GetCommetText commentText = newTextObj.GetComponent<GetCommetText>();
        RectTransform rectTransform = newTextObj.GetComponent<RectTransform>();

        // �e�L�X�g��ݒ�
        commentText.SetCommentText(selectedText);

        rectTransform.sizeDelta = new Vector2(commentText.GetTextBoxSizeWidth(), commentText.GetTextBoxSizeHeight());
        rectTransform.anchoredPosition = DecideSpawnPos(rectTransform,raneNum);
    }
}
