//êßçÏé“Å@éõë∫

using UnityEngine;
using UnityEngine.UI;

public class CommentIntercept : MonoBehaviour
{
    CommentSpawn commentSpawn;
    Image interceptImage;
    Image maskImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        commentSpawn=GameObject.FindWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        interceptImage=this.GetComponent<Image>();
        maskImage=this.transform.GetChild(0).GetComponent<Image>();

        CheckInterceptEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInterceptEnemy();
    }

    void CheckInterceptEnemy()
    {
        if(commentSpawn.interceptEnemyIsExist)
        {
            interceptImage.enabled = true;
            maskImage.enabled = true;
        }
        else if(!commentSpawn.interceptEnemyIsExist)
        {
            interceptImage.enabled = false;
            maskImage.enabled = false;
        }
    }
}
