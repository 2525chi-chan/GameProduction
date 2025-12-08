//作成者　寺村

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodSystem : MonoBehaviour
{
    [Header("いいね数のテキスト")]
    [SerializeField] TextMeshProUGUI goodText;

    [Header("いいね獲得数のテキスト")]
    public TextMeshProUGUI addGoodText;

    [Header("いいね減少数のテキスト")]
    [SerializeField] TextMeshProUGUI decreaseGoodText;

    [Header("加算までの待機時間（待機時間がある場合）")]
    [SerializeField] float getDuration;

    [Header("待機時間を持つかどうか")]
    [SerializeField] bool enableWait=true;

    [Header("いいね数加算挙動の完了までの時間")]
    [SerializeField] int duration = 1;

    private int goodNum;    //いいね数
    public float GoodNum => goodNum; //いいね数のゲッター

    private float addGoodNum;   //最終獲得いいね取得数
    public float AddGoodNum => addGoodNum;  //最終獲得いいね数のゲッター

    private int decreaseGoodNum;

    bool decreasing;
    
    public bool Decreasing=>decreasing;

    private bool isTracking = false;    //獲得待機中か確認用フラグ

    private float displayGoodNum;   //ディスプレイ用のいいね数

    public float DisplayGoodNum => displayGoodNum;

    bool changing;  //ディスプレイいいね数の変更が終わったか

    float diff; //ディスプレイ用とオリジナルの差分

    BuzuriRank buzuriRank;

    // Start is called before the first frame update
    void Start()
    {
        buzuriRank = this.GetComponent<BuzuriRank>();
        goodNum = 0;    //開始時はゼロ
        displayGoodNum = goodNum;
        goodText.text=displayGoodNum.ToString();
        addGoodText.enabled = false;
        decreaseGoodText.enabled = false;
        changing= false;
        decreasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayGoodNum != goodNum&&!changing) 
        {
            diff = goodNum - displayGoodNum;
            changing = true;
        }

        if(changing)
        {
            ChangeDisplayGoodNum();
            if(displayGoodNum==goodNum)
            {
                changing = false;
            }
        }
        
        //Debug.Log("いいね数:" + goodNum);
    }

    public void AddGood(float value)    //他のクラスから呼ばれるいいね加算のための関数
    {
        addGoodNum += value;
        addGoodText.text = addGoodNum.ToString() + "×" + buzuriRank.currentBuzzRank.GoodMagnification.ToString();
        if (!isTracking) //現在取得時間中か確認
        {
            StartCoroutine(TrackAndApplyGoodNum());
        }
    }

    public IEnumerator DecreaseGood(int value)
    {
        decreasing=true;
        decreaseGoodNum -= value;
        decreaseGoodText.text = decreaseGoodNum.ToString();

        decreaseGoodText.enabled = true;

        yield return new WaitForSeconds(getDuration);
        
        decreaseGoodText.enabled=false;

        goodNum += decreaseGoodNum;

        if(goodNum<0)
        {
            goodNum = 0;
        }
        
        decreaseGoodNum = 0;
        decreasing = false;
    }

    void ChangeDisplayGoodNum() //いいね数が加算されたときに表示を変更する関数
    {
        if (diff == 0)
            return;
        if (diff > 0)   //加算の場合
        {
            // 1フレームあたりに加算すべき値
            float increment = (diff / duration) * Time.deltaTime;

            // 加算
            displayGoodNum += increment;

            // Clampで超過防止
            displayGoodNum = Mathf.Clamp(displayGoodNum, displayGoodNum, goodNum);

            // 表示は整数で
            goodText.text = ((int)displayGoodNum).ToString();
        }
        else if(diff < 0)   //減算の場合
        {
            // 1フレームあたりに加算すべき値
            float increment = (diff / duration) * Time.deltaTime;

            // 加算
            displayGoodNum += increment;

            // Clampで超過防止
            displayGoodNum = Mathf.Clamp(displayGoodNum,goodNum ,displayGoodNum );

            // 表示は整数で
            goodText.text = ((int)displayGoodNum).ToString();
        }
    }

    private IEnumerator TrackAndApplyGoodNum()  //最終獲得いいね数を取得し、現在のいいね数に加算する関数
    {
        isTracking = true;

        addGoodText.enabled = true;

        if (enableWait)
        {
            yield return new WaitForSeconds(getDuration);
        }
         //一定時間この関数の処理を止めるコルーチン

        addGoodNum *= buzuriRank.currentBuzzRank.GoodMagnification; //最終獲得いいね数に現在のバズリランクのいいね取得倍率をかける

        addGoodText.enabled = false;

        goodNum += (int)addGoodNum; //現在のいいね数に最終獲得いいね数を加算

        addGoodNum = 0; //最終獲得いいね数をリセット

        isTracking = false;
    }
}