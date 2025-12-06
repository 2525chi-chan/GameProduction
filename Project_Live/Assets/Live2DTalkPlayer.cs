using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class TalkEntry
{
    public string texts;
    public float delayBefore = 0f;//セリフ再生前の待機時間
    public float delayEnd = 0f;//セリフ再生後の待機時間
    public AudioClip audioClip;
}

[System.Serializable]
public class TalkData//特定のアクションに対するセリフデータ
{
    public string motionName;
    public bool isBreak;
    public List<TalkEntry> talkEntries;
}

public class Live2DTalkPlayer : MonoBehaviour
{

    [SerializeField] TMP_Text talkText;
    [SerializeField] float textSpeed = 0.05f;
    [SerializeField] List<TalkData> talkDatas = new();

    public List<TalkData> TalkDatas { get { return talkDatas; } }
    Live2DController live2DController;

    Coroutine showTextCoroutine;
    int cuttentTalkIndex = -1;
    float waitStartTime;
    float waitEndTime;
    //private void OnValidate()
    //{
    //    live2DController = GetComponent<Live2DController>();
    //    if (live2DController == null || live2DController.Motions == null)
    //        return;

    //    var motions = live2DController.Motions;
    //    int count = motions.Count;

    //    // talkDatas のサイズを Motions に合わせる
    //    if (talkDatas.Count < count)
    //    {
    //        // 足りない分だけ追加
    //        int diff = count - talkDatas.Count;
    //        for (int i = 0; i < diff; i++)
    //        {
    //            talkDatas.Add(new TalkData());
    //        }
    //    }
    //    else if (talkDatas.Count > count)
    //    {
    //        // 余っている分を削る
    //        talkDatas.RemoveRange(count, talkDatas.Count - count);
    //    }

    //    // 共通で、motionName だけ同期
    //    for (int i = 0; i < count; i++)
    //    {
    //        talkDatas[i].motionName = motions[i].motionName;
    //        // voice や subtitle など他フィールドは既存値を維持
    //    }
    //}

    public void PlayTalk(string motionName)//セリフ再生
    {



        if (cuttentTalkIndex >= 0 && !talkDatas[cuttentTalkIndex].isBreak && showTextCoroutine != null) return;//現在再生中のセリフが中断不可の場合、コルーチンが終了するまで新たなセリフ再生を拒否
        else
        {

            if (showTextCoroutine != null)
            {
                StopCoroutine(showTextCoroutine);
                showTextCoroutine = null;
            }
            talkText.text = "";
        }

        foreach (var data in talkDatas)
        {
            if (data.motionName == motionName)
            {
                var rand = Random.Range(0, data.talkEntries.Count);
                waitStartTime = data.talkEntries[rand].delayBefore;
                waitEndTime=data.talkEntries[rand].delayEnd;
                cuttentTalkIndex = talkDatas.IndexOf(data);
                showTextCoroutine = StartCoroutine(ShowText(data.talkEntries[rand].texts));
                break;
            }
        }
    }


    public IEnumerator ShowText(string text)
    {
        yield return new WaitForSeconds(waitStartTime);
        Queue<char> chars = new Queue<char>(text.ToCharArray());
        talkText.text = "";
        while (chars.Count > 0)
        {
            talkText.text += chars.Dequeue();
            yield return new WaitForSeconds(textSpeed);
        }
        showTextCoroutine = null;

        yield return new WaitForSeconds(waitEndTime);

    }
}
