using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CommentReplyArea : MonoBehaviour
{

    [HideInInspector]
    public List<GameObject> canReplyComment;

    public RectTransform replyAreaRect { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        replyAreaRect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canReplyComment.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(SetReplyComment(canReplyComment));
        }
    }

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
