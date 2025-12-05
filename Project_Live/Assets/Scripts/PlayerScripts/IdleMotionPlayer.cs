using Live2D.Cubism.Framework.Motion;
using UnityEngine;
using Live2D.Cubism.Framework.Expression;
using System.Collections.Generic;
public class IdleLive2DPlayer : MonoBehaviour//待機中にLive2Dを動かす
{
    [Header("何もしていない時に再生するモーション")]
    [SerializeField]float waitTime = 5f;//一定時間経過したら特別なIdleを再生する
    [Header("デフォルトのモーション(呼吸)")]
    [SerializeField] string defaultAnimation;
    Live2DController live2DController;
    CubismMotionController controller;
    List<MotionData> idleMotions = new List<MotionData>();
    bool isPlayingDefault = false;
    void Start()
    {
       live2DController = GetComponent<Live2DController>();
        controller =live2DController.MotionController;
        controller.AnimationEndHandler+=OnMotionEnd;

        foreach (var mot in live2DController.Motions)
        {
            if (mot.motionName.Contains("Idle"))
            {
                idleMotions.Add(mot);
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
                countTime = 0f;
                live2DController.PlayMotion(idleMotions[rand].motionName);

                isPlayingDefault = false;
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
