//制作者　寺村
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CommentIntercept : MonoBehaviour
{
    [Header("必要なコンポーネント")]
    [SerializeField] TextMeshProUGUI commentText;
    [SerializeField] float fadeDuration = 1f;          // フェード時間
    //[SerializeField] Image targetImage;                // 通常Image用
    [SerializeField] Material targetMaterial;          // Shaderマテリアル用

    bool fadeFlag;

    CommentSpawn commentSpawn;
    Image interceptImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        commentSpawn=GameObject.FindWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        interceptImage=this.GetComponent<Image>();

        fadeFlag = false;

        targetMaterial.SetFloat("_Alpha", 0.0f);

        if(commentSpawn.interceptEnemyIsExist)
        {
            commentText.alpha = 0.0f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckInterceptEnemy();
        if(commentSpawn.interceptEnemyIsExist&&!fadeFlag)
        {
            StartCoroutine(FadeInOrOut(1f,0f));
        }
        if (!commentSpawn.interceptEnemyIsExist && fadeFlag)
        {
            StartCoroutine(FadeInOrOut(0f, 1f));
        }
    }

    

   void CheckInterceptEnemy()
    {
        if(commentSpawn.interceptEnemyIsExist)
        {
            interceptImage.enabled = true;
            //maskImage.enabled = true;
        }
        else 
        {
            interceptImage.enabled = false;
            commentText.alpha = 1f;
            //maskImage.enabled = false;
        }
    }

    IEnumerator FadeInOrOut(float endAlphaImage,float endAlphaText)
    {
        float elapsed = 0f;
        float startAlphaImage = GetCurrentAlpha(targetMaterial);
        float startAlphaText = GetCurrentAlpha(null);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alphaImage = Mathf.Lerp(startAlphaImage,endAlphaImage, elapsed / fadeDuration);
            SetAlpha(alphaImage,targetMaterial);
            float alphaText = Mathf.Lerp(startAlphaText, endAlphaText, elapsed / fadeDuration);
            SetAlpha(alphaText, null);
            yield return null;
        }
        SetAlpha(endAlphaText, null) ;
        SetAlpha(endAlphaImage,targetMaterial);
        
        fadeFlag = true;
    }

    // 現在のalpha取得（ImageかMaterialか判定）
    float GetCurrentAlpha(Material material)
    {
        if (material == targetMaterial && targetMaterial.HasProperty("_Alpha"))
            return targetMaterial.GetFloat("_Alpha");
        else if(material==null)
            return commentText.alpha;
        return 0f;
    }

    // alpha設定（ImageかMaterialか判定）
    void SetAlpha(float alpha,Material material)
    {
        if (material==targetMaterial && targetMaterial.HasProperty("_Alpha"))
            targetMaterial.SetFloat("_Alpha", alpha);
        else if(material==null)
        {
            commentText.alpha = alpha;
        }
    }
}
