using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//作成者　寺村

[System.Serializable]
public class BuzzRank
{
    [Header("バズリランク名")]
    [SerializeField] string name;

    [Header("このバズリランクに必要ないいね数")]
    [SerializeField] public float needNum;
    [Header("このバズリランクのいいね取得倍率")]
    [SerializeField] float goodMagnification;
    public float GoodMagnification => goodMagnification;
    [Header("このバズリランクのコメントのスポーン時間")]
    [SerializeField] float commentSpawnTime;
    public float CommentSpawnTime => commentSpawnTime;    //スポーン時間参照用ゲッター
    [Header("このバズリランクの色")]
    [SerializeField] public Color rankColor;
    [Header("このバズリランクで表示されるコメント")]
    [SerializeField] List<string> commentContent = new List<string>();
    public List<string> CommentContent => commentContent;
    public int comentNum=>CommentContent.Count;

}

public class BuzuriRank : MonoBehaviour
{
    [Header("バズリランク設定")]
    [SerializeField] List<BuzzRank> buzzRanks;
    [Header("バズリランクゲージバー")]
    [SerializeField] Slider BuzuriSlider;
    [Header("必要なコンポーネント")]
    [SerializeField] Image buzuriGage;

    private int currentIndex = 0;   //現在のクラスのインデックス数
    private float beforeMaxValue = 0;   //前のバズリランクに到達するまでに必要ないいね数

    [System.NonSerialized]
    public BuzzRank currentBuzzRank = new BuzzRank();   //現在のバズリランクを格納しておくインスタンス

    GoodSystem goodSystem;

    // Start is called before the first frame update
    void Start()
    {
        goodSystem = this.GetComponent<GoodSystem>();
        currentBuzzRank = buzzRanks[currentIndex];  //最初のバズリランク情報を格納
        buzuriGage.color = currentBuzzRank.rankColor;
        goodSystem.addGoodText.color=currentBuzzRank.rankColor;
        BuzuriSlider.maxValue = buzzRanks[currentIndex + 1].needNum;  //バズリランクゲージの最大値を次のバズリランクに必要ないいね数にする
    }

    // Update is called once per frame
    void Update()
    {
        BuzuriSlider.value = goodSystem.GoodNum - beforeMaxValue; //いいね数から現在のバズリランクまでに必要だったいいね数を引く

        if (currentIndex + 1 < buzzRanks.Count) //次のバズリランクが設定されていれば実行
        {
            if (goodSystem.GoodNum >= buzzRanks[currentIndex + 1].needNum)  //いいね数が次のバズリランクに必要な数に到達したら実行
            {
                currentIndex++; //次のインデックス数に移動
                currentBuzzRank = buzzRanks[currentIndex];  //バズリランクを上げる
                buzuriGage.color = currentBuzzRank.rankColor;    
                goodSystem.addGoodText.color = currentBuzzRank.rankColor;   //バズリランクゲージといいね獲得数を現在のバズリランクの色にする
                if (currentIndex + 1 != buzzRanks.Count)    //設定されている最高のバズリランクに到達しているか確認
                {
                    BuzuriSlider.maxValue = currentBuzzRank.needNum - buzzRanks[currentIndex - 1].needNum;    //ゲージをリセットさせるための処理
                    beforeMaxValue = currentBuzzRank.needNum;   //現在のバズリランクまでに必要だったいいね数代入
                }
                Debug.Log("いいね数が" + currentBuzzRank.needNum + "を超えたのでバズリランクをあげ、いいね取得倍率を" + currentBuzzRank.GoodMagnification + "、コメントのスポーン時間を" + currentBuzzRank.CommentSpawnTime + "秒に変更しました。");
            }
        }
    }
}
