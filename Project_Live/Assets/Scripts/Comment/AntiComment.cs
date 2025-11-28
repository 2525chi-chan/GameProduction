//　制作者　寺村

using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AntiComment : MonoBehaviour
{
    public enum DecreaseType
    { 
        Fixed,
        Pecentage,
        Mixed
    }

    [Header("固定値で減らすか、パーセンテージで減らすか、混合か")]
    [SerializeField] DecreaseType decreaseType;
    [Header("受け取ったら減らすいいね数(固定値)")]
    [SerializeField] int[] fixedValueDecrease;
    [Header("受け取ったら減らすいいね数(割合)")]
    [SerializeField, Range(0, 100)] int decreaseRate;
    [Header("何いいね以上で減少方法を固定値→割合に変更するか")]
    [SerializeField] int changeDecreaseTypeNum;
    [Header("アンチコメントの内容")]
    public List<string> antiCommentContent = new List<string>();
    [Header("決定時のエフェクト")]
    [SerializeField] ParticleSystem pressEffect;

    bool Pressed;

    GoodSystem goodSystem;
    Button operationDetail;
    CommentMove commentMove;
    CommentSpawn commentSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goodSystem = GameObject.FindGameObjectWithTag("GoodSystem").GetComponent<GoodSystem>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        commentMove=this.GetComponent<CommentMove>();
        operationDetail=this.GetComponent<Button>();
        operationDetail.onClick.RemoveAllListeners();
        operationDetail.onClick.AddListener(DecreaseGoodNum);
        Pressed = false;
        commentSpawn.antiCommentIsExist = true;
    }

    void Update()
    {
        if (Pressed && !goodSystem.Decreasing)
        {
            Destroy(this.gameObject);
        }
    }

    void PressMethod()  //押された瞬間のエフェクトやコメント移動の停止関数
    {
        Pressed = true;
        pressEffect.Play();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;
    }

    int MixedDecrease() //混合減少の関数
    {
        int value;

        if(goodSystem.GoodNum>=changeDecreaseTypeNum)
        {
            return value=PercentageDecrease();
        }
        else
        {
            return value=FixedDecrease();
        }
    }

    int FixedDecrease() //固定値減少の関数
    {
        int value= fixedValueDecrease[Random.Range(0, fixedValueDecrease.Length)];
        Debug.Log("アンチコメントによるいいねの減少量は" + value + "です。");

        return value;
    }

    int PercentageDecrease()    //％減少の関数
    {
        int value= (int)(goodSystem.GoodNum * decreaseRate / 100);
        Debug.Log("減少率が" + decreaseRate + "%と設定されているため、いいね数" + goodSystem.GoodNum + "から減少量は" + value + "です。");
        
        return value;
    }


    public void DecreaseGoodNum ()  //コメントが拾われたら行われる関数
    {
        PressMethod();

        int decreaseValue = 0;

        switch (decreaseType)
        {
            case DecreaseType.Fixed:
                decreaseValue = FixedDecrease();
                    break;
            case DecreaseType.Pecentage:
                decreaseValue = PercentageDecrease();
                    break;
            case DecreaseType.Mixed:
                decreaseValue = MixedDecrease();
                break;
        }

        StartCoroutine(goodSystem.DecreaseGood(decreaseValue));

        //goodSystem.DecreaseGood(decreaseValue);
    }

    void OnDestroy()
    {
        commentSpawn.antiCommentIsExist = false;   
    }
}
