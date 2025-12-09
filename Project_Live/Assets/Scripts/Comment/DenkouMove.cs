using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DenkouMove : MonoBehaviour
{
    [Header("電光掲示板に表示される時間")]
    [SerializeField] float existSeconds;

    RequestManager requestManager;
    Image denkouImg;

    public bool thisIsEnemy { set; private get; }
    float perFrameMoveSpeed;
    RectTransform imgRect, thisRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        requestManager = GameObject.FindGameObjectWithTag("RequestManager").GetComponent<RequestManager>();
        denkouImg = GameObject.FindGameObjectWithTag("Denkou").GetComponent<Image>();
        imgRect = denkouImg.GetComponent<RectTransform>();
        thisRect=this.GetComponent<RectTransform>();

        requestManager.denkouDisplay = true;
        perFrameMoveSpeed = ((imgRect.rect.width ) / existSeconds) / Application.targetFrameRate;
        //Debug.Log("FPS:" + Application.targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {
        thisRect.anchoredPosition -= new Vector2(perFrameMoveSpeed, 0);
        if(thisIsEnemy)
        {
            this.GetComponent<TextMeshProUGUI>().text = "あと" + (requestManager.currentRequestEnemy.defeatEnemyNum - requestManager.currentRequestEnemy.enemyCounter) + "体敵を倒そう!!!";
        }
        if (!IsTextRightEdgeLeftOfImage()||requestManager.addRequest||requestManager.isAnimating||requestManager.isIntercepting)
        {
            requestManager.denkouDisplay = false;
            Destroy(this.gameObject);
        }
    }

    bool IsTextRightEdgeLeftOfImage()
    {
        // floatでX座標のみ計算（Yは不要）
        float imageLeftEdge = imgRect.anchoredPosition.x - imgRect.rect.width / 2f;
        float textRightEdge = thisRect.anchoredPosition.x + thisRect.rect.width / 2f;

        return textRightEdge > imageLeftEdge;  // Text右端がImage左端より右なら生存
    }
}
