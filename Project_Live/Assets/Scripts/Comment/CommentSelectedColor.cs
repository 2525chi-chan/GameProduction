using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CommentSelectedColor : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("コメント選択時の色")]
    [SerializeField]Color selectedColor;
    [Header("コメント未選択時の色")]
    [SerializeField]Color normalColor;

    [Header("必要なコンポーネント")]
    [SerializeField]TextMeshProUGUI targetText;
    [SerializeField]Outline outline;
    [SerializeField] Animator animator;

    public void OnSelect(BaseEventData eventData)
    {
        animator.SetBool("InArea", true);
        outline.enabled = true;
        outline.effectColor = selectedColor;
        //animator.Play("SelectHighlight");
        //  targetText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        animator.SetBool("InArea", false);
        //animator.SetFloat("AnimationSpeed", 0.0f);
        outline.enabled = false;
        
      //  targetText.color = normalColor;
    }
}
