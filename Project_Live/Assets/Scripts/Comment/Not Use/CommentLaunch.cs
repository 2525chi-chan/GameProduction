//作成者：寺村
//放物移動については{https://www.gocca.work/unity-parabolic-movement/}から引用

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentLaunch : MonoBehaviour
{
    [Header("ステージ")]
    [SerializeField] GameObject stage;
    [Header("滞空時間")]
    [SerializeField] float flightTime = 2f;
    [Header("移動速度倍率")]
    [SerializeField] float speedRate = 1f;
    
    private const float gravity = -9.8f;    //重力
    BoxCollider commentCollider;    //浮遊中は取れないようにするためにコライダーを取得

    // Start is called before the first frame update
    void Start()
    {
        Vector3 center = stage.transform.position + Vector3.up * stage.transform.localScale.y * 0.5f; // ステージの上面中心
        float radius = 0.5f * stage.transform.localScale.x; // 円柱のスケールから半径を取得

        Vector3 randomTarget = GetRandomPointOnCylinderTop(center, radius);

        // オブジェクトの高さ分だけY座標を上げる
        float objectHeight = GetObjectHeight(this.gameObject);
        randomTarget.y += objectHeight * 0.5f; // だいたい中心から底面まで半分なので0.5倍

        commentCollider=GetComponent<BoxCollider>();

        commentCollider.enabled = false;    //生成された瞬間はコライダーが無効

        StartCoroutine(Launch(randomTarget, flightTime, speedRate, gravity));   //生成されたら放物運動を開始
    }

    float GetObjectHeight(GameObject obj)
    {
        var collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size.y;
        }
        else
        {
            // Colliderない場合は適当なサイズかデフォルト
            return 1f; // 仮に1m扱い
        }
    }
    Vector3 GetRandomPointOnCylinderTop(Vector3 center, float radius)
    {
        // 中心からランダムな距離・角度で点を取る
        float angle = Random.Range(0f, Mathf.PI * 2f); // 0～360度
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // 面積均等になるように√を取る

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        return new Vector3(center.x + x, center.y, center.z + z);
    }

    private IEnumerator Launch(Vector3 targetPos, float flightTime, float speedRate, float gravity) //放物移動の関数
    {
        var startPos = this.transform.position; //スポーン位置
        var diffY = (targetPos - startPos).y;   //着地点とスポーン地点のy座標の差
        var v = (diffY - 0.5f * gravity * flightTime * flightTime) / flightTime;    //y方向の初速度の計算

        for (var t = 0f; t < flightTime; t += (Time.deltaTime * speedRate))     
        {
            var p = Vector3.Lerp(startPos, targetPos, t / flightTime);  //水平方向の移動距離
            p.y = startPos.y + v * t + 0.5f * gravity * t * t;  //鉛直方向の移動距離
            this.transform.position = p;    //求めた水平方向と鉛直方向へ移動
            yield return null;  //これでフレーム間で計算してくれるらしい
        }

        this.transform.position = targetPos;     //着地点へ微調整
        commentCollider.enabled = true; //着地したらコライダーを有効にする
    }
}
