using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [Header("�n�ʂɐݒu���Ă��玀�S�܂ł̎���")]
    [SerializeField] float groundStayDuration = 1f;

    [Header("�n�ʂɐݒu���Ă��邩�ǂ����𔻒肷�鋗��")]
    [SerializeField] float groundCheckDistance = 1f;

    [Header("�ڒn������s���I�u�W�F�N�g�̃��C���[")]
    [SerializeField] LayerMask groundLayer;

    [Header("���j���̃G�t�F�N�g")]
    [SerializeField] GameObject deathEffect;

    [Header("���ɐ�����΂�����")]
    [SerializeField] float knockbackForce_Back = 2f;

    [Header("��ɐ�����΂�����")]
    [SerializeField] float knockbackForce_Up = 30f;

    [Header("�����ˊl����")]
    [SerializeField] float getGoodNum;

    float groundTimer = 0f;

    bool isDead = false;
    bool isProcessing = false;

    public bool IsDead { get { return isDead; } }
    public bool IsProcessing { get { return isProcessing; } }

    Rigidbody rb;
    GoodSystem goodSystem;
    EnemySpawnManager spawnManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        goodSystem = GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();
        spawnManager = GameObject.FindWithTag("EnemySpawnManager").GetComponent<EnemySpawnManager>();
    }

    private void Update()
    {
        if (isProcessing)
        {
            if (groundStayDuration == 0f) Die();

            //Debug.Log("���S��ԂɈڍs");
            if (IsGrounded())
            {
                groundTimer += Time.deltaTime;

                //Debug.Log(groundTimer);

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
        spawnManager.DefeatedEnemyCount++;
        Destroy(gameObject);
    }

    public bool IsGrounded() //�n�ʂɐݒu���Ă��邩�ǂ����𔻒肷�鏈��
    {
        Vector3 direction = Vector3.down * groundCheckDistance;
        Debug.DrawRay(transform.position, direction, Color.red);

        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}