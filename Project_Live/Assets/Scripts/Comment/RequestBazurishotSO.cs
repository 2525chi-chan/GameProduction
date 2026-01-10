using UnityEngine;

[CreateAssetMenu(fileName = "New RequestBazuriShot", menuName = "Request/BazuriShot")]
public class RequestBazuriShotSO : RequestBaseSO
{
    [Header("バズリショットで稼いでほしい、いいね数")]
    public int requestNum;

    protected override void OnEnable()
    {
        base.OnEnable();
        requestType = RequestType.BazuriShot;
        displayText = "バズリショットで" + requestNum + "いいねを稼ごう!!!";
    }
}