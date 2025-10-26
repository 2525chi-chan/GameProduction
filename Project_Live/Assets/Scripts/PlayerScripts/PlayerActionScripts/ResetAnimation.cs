using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ResetAnimation : StateMachineBehaviour//���̃X�e�[�g�ɐ؂�ւ�鎞�ɃA�j���[�V������SpeedMultiply��1�Ƀ��Z�b�g����
{
    [SerializeField]float resetDuration = 0.2f; // ���Z�b�g�܂ł̒x������

    public void Awake()
    {
       
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
         TransformResetHelper helper=animator.gameObject.GetComponent<TransformResetHelper>();
        animator.SetFloat("SpeedMultiply", 1);
        Debug.Log("�A�j���[�V������SpeedMultiply��1�Ƀ��Z�b�g");
        helper.StartCoroutine(helper.SmoothResetTransform(animator,resetDuration));
    }

  
   
}
