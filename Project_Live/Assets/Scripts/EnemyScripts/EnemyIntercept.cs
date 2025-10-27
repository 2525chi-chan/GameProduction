using UnityEngine;

public class EnemyIntercept : MonoBehaviour
{
    CommentSpawn commentSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        commentSpawn=GameObject.FindWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        if (!commentSpawn.interceptEnemyIsExist&&commentSpawn.interceptEnemyCount==0)
        { 
            ChangeExistFlag(); 
        }

        commentSpawn.interceptEnemyCount++;
    }

    void ChangeExistFlag()
    {
       commentSpawn.interceptEnemyIsExist = !commentSpawn.interceptEnemyIsExist;
    }

    private void OnDestroy()
    {
        if (commentSpawn.interceptEnemyIsExist&&commentSpawn.interceptEnemyCount==1)
        {
            ChangeExistFlag();
        }

        commentSpawn.interceptEnemyCount--;
    }
}
