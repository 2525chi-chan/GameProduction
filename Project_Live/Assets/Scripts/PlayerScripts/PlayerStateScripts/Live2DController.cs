using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Framework.Motion;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.IO.LowLevel.Unsafe;

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
}

public class Live2DController : MonoBehaviour//Live2Dの動きと表情の制御
{
    [SerializeField] CubismMotionController motionController;
    [SerializeField]CubismExpressionController expressionController;
    [SerializeField]List<ExpressionData> expressions = new ();
    [SerializeField]List<MotionData> motions = new ();

    private int currentMotionIndex = -1;
    private void OnValidate()
    {
        if (expressions.Count > 0)
        {
            var index = 0;
            foreach(var exp in expressions)
            {

                exp.index = index;
                exp.expressionName = expressionController.ExpressionsList.CubismExpressionObjects[index].name;
                exp.expressionName = exp.expressionName.Replace(".exp3","").Replace(".exp","");
                index++;
            }
        }
    }

    public void PlayMotion(string name)//モーション再生
    {

        var priority = CubismMotionPriority.PriorityForce;
        if (motionController.IsPlayingAnimation()&&!motions[currentMotionIndex].isBreak)
        {
            
                return;
            

        }
            foreach (var mot in motions)
            {
                if (mot.motionName == name)
                {
                    Debug.Log("PlayMotion:" + name);
                    motionController.PlayAnimation(mot.animationClip, layerIndex: 0, priority: priority, isLoop: false);
                    currentMotionIndex = motions.IndexOf(mot);
                    return;
                }
            }
        
    }
  
    public void SetExpression(string name)//表情設定
    {

        foreach(var exp in expressions)
        {
            if(exp.expressionName == name)
            {
                expressionController.CurrentExpressionIndex = exp.index;
                return;
            }
        }

    }
}
