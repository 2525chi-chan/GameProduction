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

    public void OnSelect(BaseEventData eventData)
    {
        targetText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetText.color = normalColor;
    }
}
