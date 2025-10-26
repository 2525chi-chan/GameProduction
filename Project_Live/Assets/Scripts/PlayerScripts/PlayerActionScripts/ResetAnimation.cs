using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ResetAnimation : StateMachineBehaviour//他のステートに切り替わる時にアニメーションのSpeedMultiplyを1にリセットする
{
    [SerializeField]float resetDuration = 0.2f; // リセットまでの遅延時間

    public void Awake()
    {
       
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
         TransformResetHelper helper=animator.gameObject.GetComponent<TransformResetHelper>();
        animator.SetFloat("SpeedMultiply", 1);
        Debug.Log("アニメーションのSpeedMultiplyを1にリセット");
        helper.StartCoroutine(helper.SmoothResetTransform(animator,resetDuration));
    }

  
   
}
