using UnityEngine;

public class　AdaptMotionTransform : MonoBehaviour//ルートモーションの移動を親オブジェクトに適応させる
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
