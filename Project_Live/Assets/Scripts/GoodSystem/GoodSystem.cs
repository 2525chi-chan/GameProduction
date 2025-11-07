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

    [Header("加算までの待機時間")]
    [SerializeField] float getDuration;

    [Header("待機時間を持つかどうか")]
    [SerializeField] bool enableWait=true;
    private int goodNum;    //いいね数
    public float GoodNum => goodNum; //いいね数のゲッター

    private float addGoodNum;   //最終獲得いいね取得数
    public float AddGoodNum => addGoodNum;  //最終獲得いいね数のゲッター

    private bool isTracking = false;    //獲得待機中か確認用フラグ

    BuzuriRank buzuriRank;

    // Start is called before the first frame update
    void Start()
    {
        buzuriRank = this.GetComponent<BuzuriRank>();
        goodNum = 0;    //開始時はゼロ
        addGoodText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        goodText.text = goodNum.ToString();     //いいね数をstringに変換して画面に表示
        Debug.Log("いいね数:" + goodNum);
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