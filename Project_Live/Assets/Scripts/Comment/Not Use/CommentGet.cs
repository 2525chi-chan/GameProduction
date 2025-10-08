//作成者:寺村

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentGet : MonoBehaviour
{
    [Header("いいねの最小取得値")]
    [SerializeField] int minNum = 5;
    [Header("いいねの最大取得値")]
    [SerializeField] int maxNum = 10;

    GoodSystem goodSystem;
    bool getTrigger = false;    //2重取り防ぎ用のフラグ

    // Start is called before the first frame update
    void Start()
    {
        goodSystem = GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();    //スクリプトGoodSystemの変数を扱うためのおまじない
    }

    int DesideGoodNumValue(int minNum, int maxNum)
    {
        int value = Random.Range(minNum, maxNum);
        Debug.Log("コメントを取得し、いいねを" + value + "獲得しました。");
        return value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (getTrigger)
            return;
        if (other.gameObject.CompareTag("Player"))  //Playerに取られたら行われる処理  
        {
            getTrigger = true;
            Destroy(this.gameObject);
            goodSystem.AddGood(DesideGoodNumValue(minNum, maxNum));
        }
    }
}
