using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//制作者　寺村

public class CommentReplyArea : MonoBehaviour
{

    enum CommentGetType
    { 
        Selected,
        All
    }

    [Header("コメントを全取得するか、選択のみにするか")]
    [SerializeField] CommentGetType commentGetType;

    InputAction getCommentAction;

    [HideInInspector]
    public List<GameObject> canReplyComment;

    public RectTransform replyAreaRect { get; private set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        replyAreaRect = this.GetComponent<RectTransform>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(replyAreaRect.rect.width, 842);
    }

    // Update is called once per frame
    void Update()
    {
        if (canReplyComment.Count > 0&&commentGetType==CommentGetType.Selected)
        {
            EventSystem.current.SetSelectedGameObject(SetReplyComment(canReplyComment));
        }
    }

    //public void CommentGetAll(InputAction.CallbackContext context)
    //{
    //    if (canReplyComment.Count == 0||commentGetType!=CommentGetType.All)
    //        return;

    //    foreach(var comment in canReplyComment)
    //    {
    //        Button button = comment.GetComponent<Button>();
    //        button.onClick.Invoke();
    //    }
    //}

    GameObject SetReplyComment(List<GameObject> canReplyComment)
    {
        GameObject closestComment = null;
        float closestDistance = float.MaxValue;

        Vector2 areaWorldCenter = replyAreaRect.position;

        foreach (var comment in canReplyComment)
        {
            Vector2 commentWorldCenter = comment.GetComponent<RectTransform>().position;

            float distance = Vector2.Distance(areaWorldCenter, commentWorldCenter);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestComment = comment;
            }
        }
        return closestComment;
    }
}
