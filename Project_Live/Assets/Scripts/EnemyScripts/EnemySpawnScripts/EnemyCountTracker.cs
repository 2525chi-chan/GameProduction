using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountTracker : MonoBehaviour
{
    string enemyTag;
    int prev_Count;

    public EnemyCountTracker(string enemyTag)
    {
        this.enemyTag = enemyTag;
        prev_Count = 0;
    }

    int GetCurrentCount() //Enemy�^�O�����I�u�W�F�N�g�̐��𒲂ׂ�
    {
        return GameObject.FindGameObjectsWithTag(enemyTag).Length;
    }

    public bool HasChanged(out int currentCount) //�G�̐����ς�����ǂ������肷��
    {
        currentCount = GetCurrentCount();

        if (currentCount !=  prev_Count)
        {
            prev_Count = currentCount;
            return true;
        }

        return false;
    }
}
