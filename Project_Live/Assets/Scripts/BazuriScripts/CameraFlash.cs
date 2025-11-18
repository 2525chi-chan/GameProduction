
using UnityEngine;
using UnityEngine.UI;
public class CameraFlash : MonoBehaviour//バズリショットをした際のカメラフラッシュ。
{
    [Header("フラッシュ素材(画面を覆う白パネル)")]
    [SerializeField] Image flashPanel;
    [Header("フラッシュの速度")]
    [SerializeField] float flashSpeed = 5f;
    [Header("パネルの色")]
    [SerializeField] Color flashColor;

    private bool isFlashing = false;
    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            flashPanel.color = Color.Lerp(flashPanel.color, Color.clear, flashSpeed * Time.unscaledDeltaTime);
            if (flashPanel.color.a <= 0.01f)
            {

                ResetAlpha();
            }
        }
    }
    public void StartFlash()
    {
        if (!isFlashing)
        {
            isFlashing = true;

            flashPanel.color = new Color(flashColor.r, flashColor.g, flashColor.b, 1f);

        }
    }
    public void ResetAlpha()//連発時にフラッシュが起こらない現象を防ぐために、フラッシュの色をリセットする。
    {
        flashPanel.color = Color.clear;
        isFlashing = false;
    }
}
