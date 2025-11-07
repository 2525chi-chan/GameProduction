using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public struct ZoomTime
    {
    public   float zoomInTime;
       public  float zoomOutTime;
    }
public class ZoomCamera : MonoBehaviour
{

    [SerializeField] Camera baseCamera;
    [SerializeField] ZoomTime cameraZoomTime;
    [SerializeField] ZoomTime UIZoomTime;


    [SerializeField] RectTransform overlayZoomRectTransform;
    [SerializeField] RectTransform outGameRectTransform;

    [SerializeField] float overlayZoomScale = 1.5f;
    [SerializeField] float zoomWaitTime = 0.2f;
    Rect originalCameraRect;
    Rect originaloverlayUIRect;
    Rect originalUIRect;
    Rect zoomedRect = new(0, 0, 1, 1);
    Rect overlayzoomedRect = new();//画面をまたぐUIのズーム時のスケール。


    void Start()
    {
        overlayzoomedRect = new(0, 0, overlayZoomScale, overlayZoomScale);
        originalCameraRect = new(baseCamera.rect.x, baseCamera.rect.y,
            baseCamera.rect.width, baseCamera.rect.height);


        originaloverlayUIRect = new(overlayZoomRectTransform.anchoredPosition.x, overlayZoomRectTransform.anchoredPosition.y,
            overlayZoomRectTransform.localScale.x,
            overlayZoomRectTransform.localScale.y);

        originalUIRect = new(outGameRectTransform.anchoredPosition.x, outGameRectTransform.anchoredPosition.y,
            outGameRectTransform.localScale.x,
            outGameRectTransform.localScale.y);

    }
    public IEnumerator SetZoom(bool isZoomIn)
    {
        yield return new WaitForSecondsRealtime(zoomWaitTime);

        StartCoroutine(SetCameraZoom(isZoomIn));
        StartCoroutine(SetUIZoom(isZoomIn, overlayZoomRectTransform, overlayzoomedRect, originaloverlayUIRect));
        StartCoroutine(SetUIZoom(isZoomIn, outGameRectTransform, zoomedRect, originalUIRect));
        yield return null;
    }

    public IEnumerator SetCameraZoom(bool isZoomIn)//カメラのビューポート矩形を変更
    {

        float elapsedTime = 0f;
        Rect targetRect = isZoomIn ? zoomedRect : originalCameraRect;
        Rect startRect = baseCamera.rect;
        float duration = isZoomIn ? cameraZoomTime.zoomInTime : cameraZoomTime.zoomOutTime;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;

            Rect currentRect = LerpRect(startRect, targetRect, t);
            baseCamera.rect = currentRect;

            yield return null;
        }

        baseCamera.rect = targetRect;
    }
    public IEnumerator SetUIZoom(bool isZoomIn, RectTransform rect, Rect target, Rect original)//UIの矩形を変更
    {

        float elapsedTime = 0f;
        Rect targetRect = isZoomIn ? target : original;
        Rect startRect = new Rect(rect.anchoredPosition.x, rect.anchoredPosition.y, rect.localScale.x, rect.localScale.y);
        float duration = isZoomIn ? UIZoomTime.zoomInTime : UIZoomTime.zoomOutTime;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;

            Rect currentRect = LerpRect(startRect, targetRect, t);
            rect.anchoredPosition = new Vector3(currentRect.x, currentRect.y, 0);
            rect.localScale = new Vector3(currentRect.width, currentRect.height, 1);

            yield return null;
        }

        rect.anchoredPosition = targetRect.position;
        rect.localScale = new Vector3(targetRect.width, targetRect.height, 1);
    }

    public Rect LerpRect(Rect start, Rect target, float t)
    {
        t = 1 - (1 - t) * (1 - t);
        return new Rect(
        Mathf.Lerp(start.x, target.x, t),
        Mathf.Lerp(start.y, target.y, t),
        Mathf.Lerp(start.width, target.width, t),
        Mathf.Lerp(start.height, target.height, t)
    );

    }


}