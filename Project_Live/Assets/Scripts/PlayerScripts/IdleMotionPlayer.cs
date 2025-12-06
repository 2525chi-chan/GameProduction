using Live2D.Cubism.Framework.Motion;
using UnityEngine;
using Live2D.Cubism.Framework.Expression;
using System.Collections.Generic;
public class IdleLive2DManager : MonoBehaviour//待機中にLive2Dを動かす
{
    [Header("待機時間(下限)")]
    [SerializeField]float waitTime_Min = 5f;//一定時間経過したら特別なIdleを再生する
    [Header("待機時間(上限)")]
    [SerializeField] float waitTime_Max = 15f;
    [Header("デフォルトのモーション(呼吸)")]
    [SerializeField] string defaultAnimation;
    Live2DController live2DController;
    Live2DTalkPlayer talkPlayer;
    CubismMotionController controller;
    List<MotionData> idleMotions = new List<MotionData>();
    List<TalkData>idleTalks=new List<TalkData>();
    bool isPlayingDefault = false;

    float waitTime;
    void Start()
    {
        waitTime = Random.Range(waitTime_Min, waitTime_Max);

        talkPlayer = GetComponent<Live2DTalkPlayer>();
        live2DController = GetComponent<Live2DController>();
        controller =live2DController.MotionController;
        controller.AnimationEndHandler+=OnMotionEnd;

        foreach (var mot in live2DController.Motions)
        {
            if (mot.motionName.Contains("Idle"))
            {
                idleMotions.Add(mot);
                idleTalks.Add(talkPlayer.TalkDatas.Find(t => t.motionName == mot.motionName));
                Debug.Log(idleTalks.Count);
               
            }
        }
        SetDefault();
    }

    float countTime;
    // Update is called once per frame
    void Update()
    {
        if (!controller.IsPlayingAnimation())
        {
          //  live2DController.PlayMotion(defaultAnimation);
            Debug.Log("PlayDefault");
           // isPlayingDefault = true;
            SetDefault();
            return;
        }

        if (!isPlayingDefault)
        {
            countTime = 0f;
        }
        else
        {
            countTime += Time.deltaTime;
            if (countTime >= waitTime)
            {
                var rand = Random.Range(0, idleMotions.Count);
               
                live2DController.PlayMotion(idleMotions[rand].motionName);
                talkPlayer.PlayTalk(idleTalks[rand].motionName);
               
                
                isPlayingDefault = false; 
                countTime = 0f;
                waitTime = Random.Range(waitTime_Min, waitTime_Max);
            }
        }
           
        

    }
    public void SetDefault()
    {
        live2DController.PlayMotion(defaultAnimation);
        isPlayingDefault = true;
    }
  
     void OnMotionEnd(int instanceld)//呼吸をループさせる
    {
        if (live2DController.CurrentPlayingMotion == defaultAnimation)
        {
            live2DController.PlayMotion(defaultAnimation);
        }
        else
        {
            isPlayingDefault = false;
        }


            Debug.Log("saaa");
    }

   
}
