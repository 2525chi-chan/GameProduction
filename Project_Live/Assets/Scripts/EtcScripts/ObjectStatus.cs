using UnityEngine;

public class ObjectStatus : CharacterStatus
{
    [Header("�j�󎞂ɔ�������G�t�F�N�g")]
    [SerializeField] GameObject destroyEffect;

    bool isBroken = false;

    public bool IsBroken { get { return isBroken; } }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 && !isBroken)
        {
            DestroyProcess();
        }
    }

    void DestroyProcess()
    {
        if (!isBroken)
        {
            isBroken = true; //�j�󔻒�
            if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity); //�G�t�F�N�g�̐���
            Destroy(gameObject); //���̌�����j�󂷂�
        }
    }
}
