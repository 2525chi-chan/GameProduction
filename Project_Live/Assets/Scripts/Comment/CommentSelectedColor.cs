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

    public void OnSelect(BaseEventData eventData)
    {
     outline.effectColor = selectedColor;
        //  targetText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outline.effectColor = normalColor;
      //  targetText.color = normalColor;
    }
}
