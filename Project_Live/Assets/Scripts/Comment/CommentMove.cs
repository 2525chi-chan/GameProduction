using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//作成者　寺村

public class CommentMove : MonoBehaviour
{
    [Header("コメントが画面にいる秒数")]
    [SerializeField] float existSeconds;

    RectTransform rectTransform;
    float perFrameMoveSpeed;

    void Start()
    { 
        rectTransform = GetComponent<RectTransform>();
        perFrameMoveSpeed = ((Screen.width + rectTransform.sizeDelta.x) / existSeconds) / Application.targetFrameRate;  
    }

    void Update()
    {
        rectTransform.position -= new Vector3(perFrameMoveSpeed,0,0); 

        if (!IsPartOfRectTransformInsideScreen(rectTransform))
        {
            Destroy(this.gameObject);
            //Debug.Log("UI Text is outside the screen.");
        }
    }

    bool IsPartOfRectTransformInsideScreen(RectTransform rect)
    {
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                screenPoint.y >= 0 && screenPoint.y <= Screen.height)
            {
                return true; // 少なくとも1点が画面内
            }
        }

        if (worldCorners[2].x>=Screen.width)
        {
            return true;
        }

        return false; 
    }
}
