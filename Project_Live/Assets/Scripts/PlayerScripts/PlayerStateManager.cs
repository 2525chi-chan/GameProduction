using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateManager;

//�쐬�ҁF�K��

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    public struct StateFlags
    {
        public bool isInvincible;
    }

    [Header("�}�e���A���̕ύX���s���I�u�W�F�N�g")]
    [SerializeField] Renderer playerRenderer;

    [Header("�ʏ펞�p�̃}�e���A��")]
    [SerializeField] Material normalMaterial;
    [Header("���G��Ԏ��p�̃}�e���A��")]
    [SerializeField] Material invincibleMaterial;

    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] Dodge dodge;

    StateFlags currentStateFlags;
    StateFlags previousStateFlags;

    Renderer targetRenderer;

    void Start()
    {
        targetRenderer = playerRenderer.GetComponent<Renderer>();
    }

    void Update()
    {
        currentStateFlags.isInvincible = dodge.IsDodging; //���݂̉����Ԃ��X�V

        CheckAndLogStateChange();

        UpdateMaterialOnState();
    }

    private void CheckAndLogStateChange() //��ԕω��ɉ���������
    {
        if (currentStateFlags.isInvincible != previousStateFlags.isInvincible)
        {
            if (currentStateFlags.isInvincible)
                Debug.Log("���G�ł��I");
        }

        previousStateFlags = currentStateFlags;
    }

    private void UpdateMaterialOnState() //��Ԃɂ���ă}�e���A����ύX���鏈��
    {
        if (currentStateFlags.isInvincible)
        {
            targetRenderer.material = invincibleMaterial;
        }

        else
            targetRenderer.material = normalMaterial;
    }
}
