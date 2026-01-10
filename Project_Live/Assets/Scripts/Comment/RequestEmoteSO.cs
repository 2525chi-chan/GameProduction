using UnityEngine;

[CreateAssetMenu(fileName = "New RequestEmote", menuName = "Request/Emote")]
public class RequestEmoteSO : RequestBaseSO
{
    public bool doneEmote { set; private get; }

    protected override void OnEnable()
    {
        base.OnEnable();
        requestType = RequestType.Emote;
        displayText = "ÉGÉÇÅ[ÉgÇÇµÇÊÇ§!!!";
    }
}
