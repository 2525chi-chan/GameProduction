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
  

    float waitTime;


    MotionData pendingMotion;
    TalkData pendingTalk;
    bool hasPending = false;
    void Start()
    {
        if(controller==null) return;

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
        if(controller==null) return;

        if (!controller.IsPlayingAnimation())
        {
          //  live2DController.PlayMotion(defaultAnimation);
       //     Debug.Log("PlayDefault");
           // isPlayingDefault = true;
         
            SetDefault();
            return;
        }

      
      

            if (hasPending) return;
           
            countTime += Time.deltaTime;
            if (countTime >= waitTime)//待機時間を超えたらIdleモーションをセット
            {
                var rand = Random.Range(0, idleMotions.Count);
               

                pendingMotion = idleMotions[rand];
                pendingTalk = idleTalks[rand];
                hasPending = true;
               // live2DController.PlayMotion(idleMotions[rand].motionName);
               // talkPlayer.PlayTalk(idleTalks[rand].motionName);
               
                
               
                countTime = 0f;
                waitTime = Random.Range(waitTime_Min, waitTime_Max);
            }
        
           
        

    }
    public void SetDefault()
    {
        live2DController.PlayMotion(defaultAnimation);
       
    }
  
     void OnMotionEnd(int instanceld)//呼吸をループさせる
    {
        if (live2DController.CurrentPlayingMotion == defaultAnimation)
        {
            live2DController.PlayMotion(defaultAnimation);
        }
      
        if (hasPending)
        {
          //  Debug.Log("sasasa");

            live2DController.PlayMotion(pendingMotion.motionName);
            talkPlayer.PlayTalk(pendingTalk.motionName);
            hasPending = false;
            
        }

           // Debug.Log("saaa");
    }

   
}
