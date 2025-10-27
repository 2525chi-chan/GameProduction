using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnBoss;//生成するボス
    [SerializeField] Transform spawnPoint;//生成位置
    [SerializeField] GameObject spawnBossEffect;//ボス生成エフェクト

    public void SpawnBoss()
    {
        Instantiate(spawnBoss, spawnPoint.position, Quaternion.identity);

    }

   
}
