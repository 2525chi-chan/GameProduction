using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;

[System.Serializable]
public class VoiseData
{
    public float delayBefore;//セリフ再生前の待機時間
    public AudioClip audioClip;
}

[System.Serializable]
public class TalkEntry
{
    public string texts;//セリフ
    public float delayBefore = 0f;//セリフ再生前の待機時間
    public float delayEnd = 0f;//セリフ再生後の待機時間
   public List<VoiseData> voiseDatas;

 //   public AudioClip audioClip;
}

[System.Serializable]
public class TalkData//特定のアクションに対するセリフデータ
{
    public string motionName;
 //   public bool isBreak;
    public bool isForce;
    public bool isMain;
    public List<TalkEntry> talkEntries;
}

public class Live2DTalkPlayer : MonoBehaviour
{

    [SerializeField] TMP_Text talkText;
    [SerializeField] float subVolumeNormal = 0.3f;
    [SerializeField] float subVolumeWhileMain = 0.1f;
    [SerializeField] float textSpeed = 0.05f;
    [SerializeField] List<TalkData> talkDatas = new();
    [SerializeField] AudioSource Main_Audio;
    [SerializeField]AudioSource Sub_Audio;
    public List<TalkData> TalkDatas { get { return talkDatas; } }
    Live2DController live2DController;

    Coroutine showMainTextCoroutine;
    Coroutine showSubTextCoroutine;
    Coroutine playMainVoiseCoroutine;
    Coroutine playSubVoiseCoroutine;

    bool isMainPlaying = false;
 
    float waitStartTime;
    float waitEndTime;
  

    private void Start()
    {
        live2DController = GetComponent<Live2DController>();

    }

    private void Update()
    {
        isMainPlaying = playMainVoiseCoroutine != null || showMainTextCoroutine != null;//両方のコルーチンが終わるまで次のセリフを再生しない


        Sub_Audio.volume = Main_Audio.isPlaying ? subVolumeWhileMain : subVolumeNormal;

    }
    public void PlayTalk(string motionName)//セリフ再生
    {

        if (live2DController == null)
        {
           // Debug.Log("???");
            return;
        }

        TalkData target = talkDatas.Find(voise => voise.motionName == motionName);

        if (target == null) return;

       
        var rand = Random.Range(0, target.talkEntries.Count);
        var entry = target.talkEntries[rand];
        waitStartTime = entry.delayBefore;
        waitEndTime = entry.delayEnd;

        if (target.isMain)
        {
            if (isMainPlaying&&!target.isForce) return;

            if (showMainTextCoroutine != null)
            {
                StopCoroutine(showMainTextCoroutine);
                showMainTextCoroutine = null;
                talkText.text = "";
            }
            if (playMainVoiseCoroutine != null)
            {
                StopCoroutine(playMainVoiseCoroutine);
            }

            playMainVoiseCoroutine = StartCoroutine(PlayVoise(entry.voiseDatas, true));
            showMainTextCoroutine = StartCoroutine(ShowText(entry.texts,true));
        }
        else
        {
            if (playSubVoiseCoroutine != null)
            {
                StopCoroutine(playSubVoiseCoroutine);
                playSubVoiseCoroutine = null;

            }
            playSubVoiseCoroutine = StartCoroutine(PlayVoise(entry.voiseDatas, false));
            if (!Main_Audio.isPlaying)//字幕はメインセリフが無い場合のみ表示する
            {
                talkText.text = "";
                StartCoroutine(ShowText(entry.texts,false));
            }


        }



    }

    public IEnumerator PlayVoise(List <VoiseData> voises,bool isMain)
    {

        List<VoiseData> voiseQueue = new List<VoiseData>(voises);
        var count = 0f;
        while (voiseQueue.Count > 0)
        {
            count += Time.unscaledDeltaTime;
            if (count >= voiseQueue[0].delayBefore)
            {
                if (isMain)
                {
                    Main_Audio.PlayOneShot(voiseQueue[0].audioClip);
                }
                else
                {
                    Sub_Audio.PlayOneShot(voiseQueue[0].audioClip);
                }

                voiseQueue.RemoveAt(0);
            }


            yield return null;
        }


       
        if (isMain)
        {
            playMainVoiseCoroutine = null;
        }
        else
        {
            playSubVoiseCoroutine = null;
        }
            

    }
    public IEnumerator ShowText(string text,bool isMain)
    {
        yield return new WaitForSeconds(waitStartTime);
        Queue<char> chars = new Queue<char>(text.ToCharArray());
        talkText.text = "";
        while (chars.Count > 0)
        {
            talkText.text += chars.Dequeue();
            yield return new WaitForSeconds(textSpeed);
        }


        if (isMain)
        {
            showMainTextCoroutine = null;

        }
       
        yield return new WaitForSeconds(waitEndTime);

    }
}
