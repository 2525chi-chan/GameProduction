
using UnityEngine;
using UnityEngine.UI;

public class CameraRectUISync : MonoBehaviour
{
    public Camera targetCamera;
    public RectTransform rectTransform;
    public Canvas canvas;

    void Start()
    {
       
       

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        // ① カメラのViewportを取得（0〜1）
        Rect viewport = targetCamera.rect;

        // ② Canvasのサイズ（ピクセル）を取得
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        // ③ ビューポート→ピクセルに変換
        float px = viewport.x * canvasSize.x;
        float py = viewport.y * canvasSize.y;
        float pWidth = viewport.width * canvasSize.x;
        float pHeight = viewport.height * canvasSize.y;

        // ④ RectTransformを直接変更
        rectTransform.anchoredPosition = new Vector2(px + pWidth / 2f, py + pHeight / 2f);
        rectTransform.sizeDelta = new Vector2(pWidth, pHeight);
    }
}
