using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class EnemyStatus : CharacterStatus
{
    private void Update()
    {
        if (Hp <= 0) Die();
    }

    void Die()
    {
        //Debug.Log("�|����");
        Destroy(gameObject);
    }
}
