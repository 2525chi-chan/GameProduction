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
    [SerializeField]ZoomTime cameraZoomTime;
    [SerializeField]ZoomTime UIZoomTime;
 
   
    [SerializeField]RectTransform overlayZoomRectTransform;
    [SerializeField] RectTransform outGameRectTransform;

    [SerializeField]float overlayZoomScale=1.5f;

    Rect originalCameraRect;
    Rect originaloverlayUIRect;
    Rect originalUIRect;
    Rect zoomedRect = new(0, 0, 1, 1);
    Rect overlayzoomedRect = new();//画面をまたぐUIのズーム時のスケール。
   

    void Start()
    {
        overlayzoomedRect=new(0,0,overlayZoomScale,overlayZoomScale);

        originalCameraRect = new(baseCamera.rect.x, baseCamera.rect.y,
            baseCamera.rect.width, baseCamera.rect.height);


        originaloverlayUIRect= new (overlayZoomRectTransform.anchoredPosition.x, overlayZoomRectTransform.anchoredPosition.y,
            overlayZoomRectTransform.localScale.x,
            overlayZoomRectTransform.localScale.y);
        originalUIRect=new (outGameRectTransform.anchoredPosition.x, outGameRectTransform.anchoredPosition.y,
            outGameRectTransform.localScale.x,
            outGameRectTransform.localScale.y);

    }
    public IEnumerator SetZoom(bool isZoomIn)
    {

        StartCoroutine(SetCameraZoom(isZoomIn));
      
       StartCoroutine(SetUIZoom(isZoomIn, overlayZoomRectTransform,overlayzoomedRect,originaloverlayUIRect));
        StartCoroutine(SetUIZoom(isZoomIn, outGameRectTransform,zoomedRect,originalUIRect));
        yield return null;
    }

    public IEnumerator SetCameraZoom(bool isZoomIn)//カメラのビューポート矩形を変更
    {
        Rect targetRect = isZoomIn ? zoomedRect : originalCameraRect;
        float elapsedTime = 0f;
        float duration = isZoomIn ? cameraZoomTime.zoomInTime : cameraZoomTime.zoomOutTime;
        Rect rect = baseCamera.rect;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = elapsedTime / duration;
            //  t = Mathf.SmoothStep(0f, 1f, t);  // 緩急あり
         //   t = 1 - (1 - t) * (1 - t);
            Rect currentRect = new Rect();

            currentRect.x = Mathf.Lerp(rect.x, targetRect.x, t);
            currentRect.y = Mathf.Lerp(rect.y, targetRect.y, t);
            currentRect.width = Mathf.Lerp(rect.width, targetRect.width, t);
            currentRect.height = Mathf.Lerp(rect.height, targetRect.height, t);


            baseCamera.rect = currentRect;

            yield return null;
        }
        baseCamera.rect = targetRect;
    }
    public IEnumerator SetUIZoom(bool isZoomIn, RectTransform rect, Rect target,Rect original)//UIの矩形を変更
    {

        float elapsedTime = 0f;
        Rect targetRect = isZoomIn ? target : original;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 startScale = rect.localScale;
        float duration = isZoomIn ? UIZoomTime.zoomInTime : UIZoomTime.zoomOutTime;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;


            Rect currentRect = new Rect();

            currentRect.x = Mathf.Lerp(startPos.x, targetRect.x, t);
            currentRect.y = Mathf.Lerp(startPos.y, targetRect.y, t);
            currentRect.width = Mathf.Lerp(startScale.x, targetRect.width, t);
            currentRect.height = Mathf.Lerp(startScale.y, targetRect.height, t);

            rect.anchoredPosition = new Vector3(currentRect.x, currentRect.y, 0);
            rect.localScale = new Vector3(currentRect.width, currentRect.height, 1);



            yield return null;
        }

        rect.anchoredPosition = targetRect.position;
        rect.localScale = new Vector3(targetRect.width, targetRect.height, 1);
    }
  


}