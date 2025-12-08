using System.Collections.Generic;
using UnityEngine;

public abstract class RequestBaseSO : ScriptableObject
{
    public enum RequestType
    {
        Enemy, 
        Emote,
        BazuriShot
    }

    public RequestType requestType { protected set; get; }
    [Header("おねだりを達成したときにもらえるいいね数")]
    public int getGoodNum;
    [Header("おねだりを達成したときに変わる応援コメントの頻度の増加率")]
    [Range(0, 100)] public int cheeringIncreasePercent;
    [Header("おねだりを失敗したときに減るいいね数(*時間制限がある場合")]
    public int decreaseGoodNum;
    [Header("おねだりを失敗したときに変わるアンチコメントの頻度の増加率(*時間制限がある場合")]
    [Range(0, 100)] public int antiIncreasePercent;
    [Header("時間制限がある場合の時間")]
    public int timeLimit;
    [Header("コメントの内容")]
    public List<string> commentText;

    public string displayText {protected set; get; } //リクエスト受領後に画面内に表示されるテキスト

    protected virtual void OnEnable()
    {
        UpdateRequestType(); // 派生クラスでオーバーライド可能
    }
    protected virtual void UpdateRequestType() { /* 基底クラス用 */ }
}

[CreateAssetMenu(fileName = "New RequestEnemy", menuName = "Request/Enemy")]
public class RequestEnemySO : RequestBaseSO
{
    public enum EnemyType
    {
        All, Selected
    }

    [Header("倒す敵は何でもいいのか、指定された敵か")]
    public EnemyType enemyType;
    [Header("倒してほしい敵の数")]
    public int defeatEnemyNum;
    [Header("倒してほしい敵の種類(*Allの場合はNone")]
    public GameObject targetEnemy;
    [Header("倒してほしい敵の名前")]
    public string enemyName;

    [System.NonSerialized]
    public int enemyCounter = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        requestType = RequestType.Enemy;
        switch (enemyType)
        {
            case EnemyType.All:
                displayText = "あと"+defeatEnemyNum + "体敵を倒そう!!!";
                break;
            case EnemyType.Selected:
                displayText = enemyName + "をあと" + defeatEnemyNum + "体倒そう！！！";
                break;
        }
    }
}

[CreateAssetMenu(fileName = "New RequestEmote", menuName = "Request/Emote")]
public class RequestEmoteSO : RequestBaseSO
{
    public bool doneEmote { set; private get; }

    protected override void OnEnable()
    {
        base.OnEnable();
        requestType= RequestType.Emote;
        displayText = "エモートをしよう!!!";
    }
}

[CreateAssetMenu(fileName = "New RequestBazuriShot", menuName = "Request/BazuriShot")]
public class RequestBazuriShotSO : RequestBaseSO
{
    [Header("バズリショットで稼いでほしい、いいね数")]
    public int requestNum;

    protected override void OnEnable()
    {
        base.OnEnable();
        requestType=RequestType.BazuriShot;
        displayText = "バズリショットで" + requestNum + "いいねを稼ごう!!!";
    }
}


