using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn
{
    GameObject enemyPrefab;
    SpawnPositionGenerator spawnPositionGenerator;
    EnemyMover.MoveType moveType;

    public EnemySpawn(GameObject enemyPrefab, BoxCollider spawnArea, EnemyMover.MoveType moveType)
    {
        this.enemyPrefab = enemyPrefab;
        this.spawnPositionGenerator = new SpawnPositionGenerator(spawnArea);
        this.moveType = moveType;
    }

    public void SpawnEnemies(int count) //�G�̐���
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = spawnPositionGenerator.GetRandomPositionInsideCollider();
            GameObject enemy = GameObject.Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            EnemyMover mover = enemy.GetComponent<EnemyMover>();
            if (mover != null) mover.SetMoveType(moveType);

            EnemyIdentifier identifier = enemy.GetComponent<EnemyIdentifier>();
            if (identifier != null) identifier.Initialize(enemyPrefab);

        }
    }    
}
