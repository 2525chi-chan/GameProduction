using UnityEngine;

public class�@AdaptMotionTransform : MonoBehaviour//���[�g���[�V�����̈ړ���e�I�u�W�F�N�g�ɓK��������
{
    Transform parent;
    Animator animator;

    private void Start()
    {
        parent = transform.parent;
        animator = parent.GetComponent<Animator>();
    }


    private void OnAnimatorMove()
    {
        var  deltaPosition = animator.deltaPosition;
        var deltaRotation = animator.deltaRotation;


        //parent.localPosition += deltaPosition;
        //parent.localRotation *= deltaRotation;


    }
    
}
