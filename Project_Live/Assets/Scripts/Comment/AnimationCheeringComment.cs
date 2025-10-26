using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AnimationCheeringComment : MonoBehaviour
{
    [Header("�A�j���[�V�����b��")]
    [SerializeField] protected float animationDuration = 0.5f;
    [Header("�A�j���[�V�����J�[�u")]
    [SerializeField] protected AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected RectTransform rectTransform;
    protected RectTransform targetPosition;
    protected bool isAnimating = false;
    protected Animator animator;

    protected void OnButtonClicked(GameObject targetObj, Action onComplete = null)
    {
        // �^�[�Q�b�g�ʒu��������
        if (targetPosition == null)
        {
            if (targetObj != null)
            {
                targetPosition = targetObj.GetComponent<RectTransform>();
                //Debug.Log("�^�[�Q�b�g���ݒ肳��܂���");
            }
        }

        if (targetPosition != null && !isAnimating)
        {
            //Debug.Log("�R�����g�̈ړ����J�n���܂�");
            // �R�[���o�b�N���R���[�`���ɓn��
            StartCoroutine(AnimateToTarget(onComplete));
        }
        else
        {
            // �A�j���[�V�������Ȃ��ꍇ�ł��R�[���o�b�N�����s
            onComplete?.Invoke();
        }
    }

    protected IEnumerator AnimateToTarget(Action onComplete = null)
    {
        yield return new WaitForSeconds(0.5f);

        animator.Play("ChangeScale");

        isAnimating = true;

        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = new Vector2(
            targetPosition.anchoredPosition.x + targetPosition.sizeDelta.x,
            targetPosition.anchoredPosition.y + targetPosition.sizeDelta.y 
        );
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float curveValue = moveCurve.Evaluate(t);

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        isAnimating = false;

        // �R�[���o�b�N�����s
        onComplete?.Invoke();
    }
}
