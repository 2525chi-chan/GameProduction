using Live2D.Cubism.Framework.Motion;
using UnityEngine;
using System.Collections.Generic;

public class IdleLive2DManager : MonoBehaviour
{
    [Header("待機時間(下限)")]
    [SerializeField] float waitTime_Min = 5f;

    [Header("待機時間(上限)")]
    [SerializeField] float waitTime_Max = 15f;

    [Header("デフォルトのモーション(呼吸)")]
    [SerializeField] string defaultAnimation;

    Live2DController live2DController;
    Live2DTalkPlayer talkPlayer;
    CubismMotionController controller;

    // Idle 用モーション＆トークをペアで保持
    List<MotionData> idleMotions = new();
    List<TalkData> idleTalks = new();

    float waitTime;
    float countTime;

    MotionData pendingMotion;
    TalkData pendingTalk;
    bool hasPending = false;

    void Start()
    {
        waitTime = Random.Range(waitTime_Min, waitTime_Max);

        talkPlayer = GetComponent<Live2DTalkPlayer>();
        live2DController = GetComponent<Live2DController>();
        controller = live2DController.MotionController;

        controller.AnimationEndHandler += OnMotionEnd;

        // 「Idle」を含むモーション＋対応トークだけをペアで登録
        foreach (var mot in live2DController.Motions)
        {
            if (!mot.motionName.Contains("Idle"))
                continue;

            var talk = talkPlayer.TalkDatas.Find(t => t.motionName == mot.motionName);
            if (talk == null)
            {
                Debug.LogWarning($"Idle '{mot.motionName}' に対応する TalkData が見つかりません。");
                continue;
            }

            idleMotions.Add(mot);
            idleTalks.Add(talk);
        }

        if (idleMotions.Count == 0)
        {
            Debug.LogWarning("Idle 用モーションが1つも登録されていません。");
        }

        SetDefault();
    }

    void Update()
    {
        if (idleMotions.Count == 0)
            return;

        // すでにIdle再生待ちならタイマーは止める
        if (hasPending)
            return;

        countTime += Time.deltaTime;
        if (countTime >= waitTime)
        {
            // ランダムに1セット選ぶ（モーションとトークは同じ index）
            int rand = Random.Range(0, idleMotions.Count);

            pendingMotion = idleMotions[rand];
            pendingTalk = idleTalks[rand];

            hasPending = true;

            // 必要ならここで先行してセリフだけ鳴らすこともできる
            // talkPlayer.PlayTalk(pendingTalk.motionName);

            countTime = 0f;
            waitTime = Random.Range(waitTime_Min, waitTime_Max);
        }
    }

    public void SetDefault()
    {
        live2DController.PlayMotion(defaultAnimation);
    }

    // モーション終了時に呼ばれるハンドラ
    void OnMotionEnd(int instanceId)
    {
        // 呼吸モーションが終わったら再度呼吸をループ
        if (live2DController.CurrentPlayingMotion == defaultAnimation)
        {
            live2DController.PlayMotion(defaultAnimation);
        }

        // 待機中Idleが予約されていればここで再生
        if (hasPending)
        {
            live2DController.PlayMotion(pendingMotion.motionName);
            talkPlayer.PlayTalk(pendingTalk.motionName);
            hasPending = false;
        }
    }
}
