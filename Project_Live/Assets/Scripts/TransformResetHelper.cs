using UnityEngine;
using  System.Collections;
public class TransformResetHelper : MonoBehaviour//コルーチン呼び出し用
{
    
    public IEnumerator SmoothResetTransform(Animator animator,float resetDuration)
    {
        Vector3 firstPos = animator.transform.localPosition;
        Quaternion firstRot = animator.transform.localRotation;
        float elapsedTime = 0f;


        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;

            var resetPos = Vector3.Lerp(firstPos, Vector3.zero, t);
            var resetRot = Quaternion.Slerp(firstRot, Quaternion.identity, t);
            animator.transform.SetLocalPositionAndRotation(resetPos, resetRot);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

    }
}
