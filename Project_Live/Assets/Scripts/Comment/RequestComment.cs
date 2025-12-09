using UnityEngine;
using System;
using System.Collections.Generic;


[System.Serializable]
public abstract class RequestBase
{
    [Header("おねだりを達成したときにもらえるいいね数")]
    [SerializeField] protected int getGoodNum;
    [Header("おねだりを達成したときに変わる応援コメントの頻度の増加率")]
    [SerializeField, Range(0, 100)] protected int cheeringIncreasePercent;
    [Header("おねだりを失敗したときに減るいいね数(*時間制限がある場合")]
    [SerializeField] protected int decreaseGoodNum;
    [Header("おねだりを失敗したときに変わるアンチコメントの頻度の増加率(*時間制限がある場合")]
    [SerializeField, Range(0, 100)] protected int antiIncreasePercent;
    [Header("時間制限がある場合の時間")]
    [SerializeField] protected int timeLimit;
    [Header("コメントの内容")]
    public List<string> commentText;
}

[System.Serializable]
public class RequestBazuriShot : RequestBase
{

    [Header("バズリショットで稼いでほしい、いいね数")]
    [SerializeField] int requestNum;
}

[System.Serializable]
public class RequestEnemy : RequestBase
{
    enum RequestType
    {
        Anything,
        Selected
    }

    [Header("倒す敵は何でもいいのか、指定された敵か")]
    [SerializeField] RequestType type;
    [Header("倒してほしい敵の数")]
    [SerializeField] int defeatEnemyNum;
    [Header("倒してほしい敵の種類(*Anythingの場合はNone")]
    [SerializeField] GameObject targetEnemy;
}

[System.Serializable]
public class RequestEmote : RequestBase
{
    public bool doneEmote { set; private get; }
}

public class RequestComment : MonoBehaviour
{
    [SerializeField]List<RequestEnemy> requestEnemy;
    [SerializeField]List<RequestBazuriShot> requestBazuriShot;
    [SerializeField]List<RequestEmote> requestEmote;

    void Start()
    {
            
    }
}

