using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnBoss;//��������{�X
    [SerializeField] Transform spawnPoint;//�����ʒu
    [SerializeField] GameObject spawnBossEffect;//�{�X�����G�t�F�N�g

    public void SpawnBoss()
    {
        Instantiate(spawnBoss, spawnPoint.position, Quaternion.identity);

    }

   
}
