using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class FadeManager : MonoBehaviour//�t�F�[�h�C���E�A�E�g���Ǘ�����
{

    [SerializeField] Image fadeImage;

    [SerializeField]float fadeInSpeed = 0.02f;
    [SerializeField]float fadeOutSpeed = 0.02f;
    float fadeTimer = 0f;
  public  IEnumerator FadeIn()
    {
        fadeTimer = 0f;
        while (fadeImage.color.a < 1)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, fadeTimer / fadeInSpeed);
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

    }
    public IEnumerator FadeOut()
    {
        fadeTimer = 0f;
        while (fadeImage.color.a > 0)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutSpeed);
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }
    }


}
