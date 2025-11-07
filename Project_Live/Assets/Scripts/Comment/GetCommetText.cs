using TMPro;
using UnityEngine;

//çÏê¨é“Å@éõë∫

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

    public float GetTextBoxSizeHeight()
    {
        return text.preferredHeight;
    }

    public void SetCommentTextColor(Color conectCommentColor)
    {
        text.color = conectCommentColor;
    }
    
}
