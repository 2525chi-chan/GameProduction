using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    //[Header("HP��0�ɂȂ��Ă��������܂ł̎���")]
    //[SerializeField] float destroyDuration = 1f;

    [Header("�n�ʂɐݒu���Ă��玀�S�܂ł̎���")]
    [SerializeField] float groundStayDuration = 2f;

    [Header("���j���̃G�t�F�N�g")]
    [SerializeField] GameObject deathEffect;

    [Header("���ɐ�����΂�����")]
    [SerializeField] float knockbackForce_Back = 2f;

    [Header("��ɐ�����΂�����")]
    [SerializeField] float knockbackForce_Up = 30f;

    [Header("�����ˊl����")]
    [SerializeField] float getGoodNum;

    bool isDead = false;
    public bool IsDead { get { return isDead; } }

    bool isProcessing = false;
    public bool IsProcessing { get { return isProcessing; } }

    Rigidbody rb;
    GoodSystem goodSystem;

    float groundTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        goodSystem = GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();
    }

    private void Update()
    {
        if (isProcessing)
        {
            Debug.Log("���S��ԂɈڍs");
            if (IsGrounded())
            {
                groundTimer += Time.deltaTime;

                if (groundTimer >= groundStayDuration) Die(); //�n�ʂɐݒu���Ă����Ԃň�莞�Ԍo�ߌ�A���S���̏������s��
            }

            else groundTimer = 0f; //�o�ߎ��Ԃ̃��Z�b�g
        }
    }

    public void StartDeathProcess() //HP��0�ɂȂ������ɌĂ΂�鏈��
    {
        if (isProcessing) return;

        isProcessing = true;

        //�̂����菈��
        Vector3 backwardForce = -transform.forward * knockbackForce_Back;
        Vector3 upwardForce = Vector3.up * knockbackForce_Up;
        rb.AddForce(backwardForce + upwardForce, ForceMode.Impulse);
        //

        goodSystem.AddGood(getGoodNum);
    }

    private void Die() //���S����
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private bool IsGrounded() //�n�ʂɐݒu���Ă��邩�ǂ����𔻒肷�鏈��
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}