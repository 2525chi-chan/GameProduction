using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Framework.Motion;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.IO.LowLevel.Unsafe;
using Live2D.Cubism.Framework.MotionFade;

[System.Serializable]
public class ExpressionData
{
    public string expressionName;
    public int index;
}
[System.Serializable]
public class MotionData
{
    public string motionName;
    public AnimationClip animationClip;
    public bool isBreak=true;//中断可能か
    public bool isForcePlay=false;//強制再生か
}

public class Live2DController : MonoBehaviour//Live2Dの動きと表情の制御
{
    [SerializeField] CubismMotionController motionController;
    public CubismMotionController MotionController { get { return motionController; } }
    [SerializeField]CubismExpressionController expressionController;
   
    [SerializeField]List<ExpressionData> expressions = new ();
    [SerializeField]List<MotionData> motions = new ();
    public List<MotionData> Motions { get { return motions; } }
    private string currentPlayingMotion = "";
    public string CurrentPlayingMotion { get { return currentPlayingMotion; } }
    private int currentMotionIndex = -1;
    private void OnValidate()
    {
        if (motionController == null||expressionController==null) return; 
        
            
        
        if (expressions.Count > 0)
        {
            var index = 0;
            foreach(var exp in expressions)
            {

                if (exp != null && expressionController.ExpressionsList != null)
                {
                    exp.index = index;
                    exp.expressionName = expressionController.ExpressionsList.CubismExpressionObjects[index].name;
                    exp.expressionName = exp.expressionName.Replace(".exp3", "").Replace(".exp", "");
                    index++;
                }
             
            }
        }
    }

    public void PlayMotion(string name)//モーション再生
    {
    // if(motionController==null||expressionController==null) return;

        MotionData data = motions.Find(mot => mot.motionName == name);

        var priority = CubismMotionPriority.PriorityForce;
        if (motionController.IsPlayingAnimation() && !motions[currentMotionIndex].isBreak && !data.isForcePlay) return;

        if (!motions.Contains(data)) return;

         Debug.Log(data.motionName);
        motionController.PlayAnimation(data.animationClip, layerIndex: 0, priority: priority, isLoop: false);
        currentMotionIndex = motions.IndexOf(data);
        currentPlayingMotion = name;
        expressionController.CurrentExpressionIndex = 3;
        // SetExpression("blink");
        return;



    }
  
    public void SetExpression(string name)//表情設定
    {
        if(motionController == null || expressionController == null) return;
        foreach (var exp in expressions)
        {
            if(exp.expressionName == name)
            {

                expressionController.CurrentExpressionIndex = exp.index;
                Debug.Log("aassss");
                return;
            }
        }

    }
}
