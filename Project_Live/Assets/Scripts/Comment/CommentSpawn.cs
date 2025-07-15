using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//作成者　寺村

public class CommentSpawn : MonoBehaviour
{
    //[Header("コメントの種類")]
    //[SerializeField] List<string> commentContent=new List<string>();

    [Header("必要なコンポーネント")]
    [SerializeField] Transform Canvas;
    [SerializeField] GameObject CommentPrefab;
    [SerializeField] BuzuriRank buzuriRank;

    float spawnTime;
    int raneNum;
    int beforeRaneNum;
    bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
            InstantiateComment(raneNum);
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


    void InstantiateComment(int raneNum)
    {
        string selectedText = buzuriRank.currentBuzzRank.CommentContent[Random.Range(0,buzuriRank.currentBuzzRank.comentNum)];

        GameObject newTextObj = Instantiate(CommentPrefab, Canvas);
        TextMeshProUGUI textBoxSize=newTextObj.GetComponent<TextMeshProUGUI>();
        RectTransform rectTransform = newTextObj.GetComponent<RectTransform>();

        // テキストを設定
        TextMeshProUGUI tmp = newTextObj.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = selectedText;
        }

        rectTransform.sizeDelta = new Vector2(textBoxSize.preferredWidth, rectTransform.sizeDelta.y);
        rectTransform.position = DecideSpawnPos(rectTransform,raneNum);
    }
}
