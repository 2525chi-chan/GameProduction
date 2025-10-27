using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ExplosionAttack : MonoBehaviour
{
    [Header("生成するオブジェクト")]
    [SerializeField] GameObject explosionPrefab;
    [Header("生成するエフェクト")]
    [SerializeField] GameObject explosionEffect;
    [Header("生成する爆発の数")]
    [SerializeField] int explosionCount = 5;
    [Header("爆発の生成間隔")]
    [SerializeField] float explosionIntervalTime = 0.2f;

    [Header("爆発の生成範囲")]
    [SerializeField] BoxCollider spawnRange;
    [Header("爆発の中心を地面に接地させるか")]
    [SerializeField] bool setToGround = true;

    float currentIntervalTime = 0f;
    int currentExplosionCount = 0;
    bool isStartProcess = false;

    void Update()
    {
        if (!isStartProcess) return;

        currentIntervalTime += Time.deltaTime;

        SpawnExplosions();
    }
    public void TriggerExplosions() //処理を開始する
    {
        isStartProcess = true; //生成処理を開始する
    }

    void SpawnExplosions() //爆発の生成
    {
        if (currentIntervalTime < explosionIntervalTime) return;

        if (currentExplosionCount >= explosionCount) //爆発が一定数生成された場合
        {
            currentExplosionCount = 0;
            isStartProcess = false; //処理が生成処理が行われないようにする
            return;
        }

        Vector3 randomPos;

        randomPos = GetRandomPositionInsideCollider(); //ランダムな位置を取得する

        Instantiate(explosionPrefab, randomPos, Quaternion.identity);
        if (explosionEffect != null)
            Instantiate(explosionEffect, randomPos, Quaternion.identity);
        currentIntervalTime = 0f; //経過時間をリセットする
        currentExplosionCount++; //爆発のカウント数を増やす
    }

    Vector3 GetRandomPositionInsideCollider() //生成位置の取得
    {
        Vector3 center = spawnRange.center + spawnRange.transform.position;
        Vector3 size = Vector3.Scale(spawnRange.size, spawnRange.transform.lossyScale);

        float x = Random.Range(-size.x / 2, size.x / 2);
        float z = Random.Range(-size.z / 2, size.z / 2);
        float y = setToGround ? spawnRange.center.y : Random.Range(-size.y / 2, size.y / 2);

        return center + new Vector3(x, y, z);
    }
}
