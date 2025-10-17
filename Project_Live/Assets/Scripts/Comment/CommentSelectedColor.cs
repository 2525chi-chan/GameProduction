using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CommentSelectedColor : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("�R�����g�I�����̐F")]
    [SerializeField]Color selectedColor;
    [Header("�R�����g���I�����̐F")]
    [SerializeField]Color normalColor;

    [Header("�K�v�ȃR���|�[�l���g")]
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
