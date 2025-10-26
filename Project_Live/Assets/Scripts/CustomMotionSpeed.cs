using UnityEngine;

public class CustomMotionSpeed : StateMachineBehaviour
{
    [SerializeField] AnimationCurve curve;
    float time;
  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0f;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       time+= Time.deltaTime;
        float speed = curve.Evaluate(time);

        animator.SetFloat("MotionSpeed", speed);
    }

}
