using TMPro;
using UnityEngine;

//�쐬�ҁ@����

public class GetCommetText : MonoBehaviour
{

    public TextMeshProUGUI text;

    public void SetCommentText(string value)
    {
        text.text = value;
    }

    public float GetTextBoxSizeWidth()
    {
        return text.preferredWidth;
    }
    
}
