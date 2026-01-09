using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class VoiseData
{
    public float delayBefore;      // セリフ再生前の待機時間
    public AudioClip audioClip;
}

[System.Serializable]
public class TalkEntry
{
    public string texts;           // セリフ
    public float delayBefore = 0f; // セリフ再生前の待機時間
    public float delayEnd = 0f;    // セリフ再生後の待機時間
    public List<VoiseData> voiseDatas;
}

[System.Serializable]
public class TalkData
{
    public string motionName;
    public bool isBreak;   // 今回は未使用なら残しておいてOK
    public bool isForce;   // true なら再生中でも割り込める
    public bool isMain=true;    // true のデータだけ使う想定
    public List<TalkEntry> talkEntries;
}

public class Live2DTalkPlayer : MonoBehaviour
{
    [SerializeField] TMP_Text talkText;
    [SerializeField] float textSpeed = 0.05f;
    [SerializeField] List<TalkData> talkDatas = new();
    [SerializeField] AudioSource Main_Audio;

    public List<TalkData> TalkDatas => talkDatas;

    Live2DController live2DController;

    Coroutine showMainTextCoroutine;
    Coroutine playMainVoiseCoroutine;

    bool isMainPlaying = false; // メインのボイス or テキストが再生中か

    float waitStartTime;
    float waitEndTime;

    TalkData lastMainTalkData;

    void Start()
    {
        live2DController = GetComponent<Live2DController>();
    }

    void Update()
    {
        // メイン音声 or テキスト再生中なら true
        isMainPlaying = (playMainVoiseCoroutine != null) || (showMainTextCoroutine != null);
    }


    public void PlayTalk(string motionName)
    {
        if (live2DController == null) return;

        TalkData target = talkDatas.Find(t => t.motionName == motionName);
        if (target == null) return;

        // メイン以外は無視（復活させる）
        if (!target.isMain) return;

        // 割り込み判定: 「前回isBreak かつ今回isForce」 or 「再生中でない」
        bool canInterrupt = !isMainPlaying ||
                           (lastMainTalkData != null && lastMainTalkData.isBreak && target.isForce);

        if (!canInterrupt) return;  // 割り込み不可&再生中でないなら無視

        // 既存停止処理...
        if (showMainTextCoroutine != null)
        {
            StopCoroutine(showMainTextCoroutine);
            showMainTextCoroutine = null;
            talkText.text = "";
        }
        if (playMainVoiseCoroutine != null)
        {
            StopCoroutine(playMainVoiseCoroutine);
            playMainVoiseCoroutine = null;
        }
        Main_Audio.Stop();

        if (target.talkEntries == null || target.talkEntries.Count == 0) return;

        int rand = Random.Range(0, target.talkEntries.Count);
        var entry = target.talkEntries[rand];

        waitStartTime = entry.delayBefore;
        waitEndTime = entry.delayEnd;

        playMainVoiseCoroutine = StartCoroutine(PlayVoise(entry.voiseDatas));
        // showMainTextCoroutine = StartCoroutine(ShowText(entry.texts));

        lastMainTalkData = target;  // 今回を記録（最後）
    }

    IEnumerator PlayVoise(List<VoiseData> voises)
    {
        if (voises == null || voises.Count == 0)
        {
            playMainVoiseCoroutine = null;
            yield break;
        }

        List<VoiseData> queue = new List<VoiseData>(voises);
        float count = 0f;

        while (queue.Count > 0)
        {
            count += Time.unscaledDeltaTime;

            if (count >= queue[0].delayBefore)
            {
                if (queue[0].audioClip != null)
                {
                    Main_Audio.PlayOneShot(queue[0].audioClip);
                }
                queue.RemoveAt(0);
            }

            yield return null;
        }

        playMainVoiseCoroutine = null;
    }

    IEnumerator ShowText(string text)
    {
        yield return new WaitForSeconds(waitStartTime);

        if (string.IsNullOrEmpty(text))
        {
            showMainTextCoroutine = null;
            yield break;
        }

        Queue<char> chars = new Queue<char>(text.ToCharArray());
        talkText.text = "";

        while (chars.Count > 0)
        {
            talkText.text += chars.Dequeue();
            yield return new WaitForSeconds(textSpeed);
        }

        showMainTextCoroutine = null;

        // セリフ再生後の待機
        yield return new WaitForSeconds(waitEndTime);

        // テキスト消したければここで
        // talkText.text = "";
    }
}
