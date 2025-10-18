using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//�쐬�ҁ@����

public class CommentMove : MonoBehaviour
{
    [Header("�R�����g����ʂɂ���b��")]
    [SerializeField] float existSeconds;

    CommentSpawn commentSpawn;

    RectTransform rectTransform;
    float perFrameMoveSpeed;

    void Start()
    { 
        commentSpawn=GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        rectTransform = GetComponent<RectTransform>();
        perFrameMoveSpeed = ((commentSpawn.Canvas.GetComponent<RectTransform>().rect.width + rectTransform.sizeDelta.x) / existSeconds) / Application.targetFrameRate;  
    }

    void Update()
    {
        rectTransform.anchoredPosition -= new Vector2(perFrameMoveSpeed,0);

        //if(commentSpawn.interceptEnemyIsExist)
        //{
        //    //this.transform.GetChild(0).gameObject.SetActive(false);
        //    this.GetComponent<Image>().enabled = true;
        //}
        //else
        //{
        //    this.GetComponent<Image>().enabled = false;
        //    this.transform.GetChild(0).gameObject.SetActive(true);
        //}

        //if (!IsPartOfRectTransformInsideScreen(rectTransform))
        //{
        //    Destroy(this.gameObject);
        //    //Debug.Log("UI Text is outside the screen.");
        //}

        if (!IsPartOfRectTransformInsideCanvas(rectTransform, commentSpawn.Canvas.GetComponent<Canvas>()))
        {
            Destroy(this.gameObject);
        }
    }

    bool IsPartOfRectTransformInsideScreen(RectTransform rect) //Space-overlay�p
    {
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        foreach (Vector3 corner in worldCorners)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, corner);
            if (screenPoint.x >= 0 && screenPoint.x <= Screen.width &&
                screenPoint.y >= 0 && screenPoint.y <= Screen.height)
            {
                return true; // ���Ȃ��Ƃ�1�_����ʓ�
            }
        }

        if (worldCorners[2].x>=Screen.width)
        {
            return true;
        }

        return false; 
    }

    bool IsPartOfRectTransformInsideCanvas(RectTransform rect, Canvas canvas)   //Space-Camera�p
    {
        Camera cam = canvas.worldCamera;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

        foreach (Vector3 corner in worldCorners)
        {
            // ���[���h���W��Canvas�̃��[�J�����W�֕ϊ�
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    RectTransformUtility.WorldToScreenPoint(cam, corner),
                    cam,
                    out localPoint))
            {
                // Canvas��Rect�͈͓�������
                if (canvasRect.rect.Contains(localPoint))
                {
                    return true; // ���Ȃ��Ƃ�1�_��Canvas��
                }
            }
        }
        return false;
    }
}
