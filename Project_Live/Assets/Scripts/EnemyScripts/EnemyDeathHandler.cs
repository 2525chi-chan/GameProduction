using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    [Header("地面に設置してから死亡までの時間")]
    [SerializeField] float groundStayDuration = 1f;

    [Header("地面に設置しているかどうかを判定する距離")]
    [SerializeField] float groundCheckDistance = 1f;

    [Header("接地判定を行うオブジェクトのレイヤー")]
    [SerializeField] LayerMask groundLayer;

    [Header("撃破時のエフェクト")]
    [SerializeField] GameObject deathEffect;

    [Header("後ろに吹き飛ばされる力")]
    [SerializeField] float knockbackForce_Back = 2f;

    [Header("上に吹き飛ばされる力")]
    [SerializeField] float knockbackForce_Up = 30f;

    [Header("いいね獲得数")]
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

            //Debug.Log("死亡状態に移行");
            if (IsGrounded())
            {
                groundTimer += Time.deltaTime;

                //Debug.Log(groundTimer);

                if (groundTimer >= groundStayDuration) Die(); //地面に設置している状態で一定時間経過後、死亡時の処理を行う
            }

            else groundTimer = 0f; //経過時間のリセット
        }
    }

    public void StartDeathProcess() //HPが0になった時に呼ばれる処理
    {
        if (isProcessing) return;

        isProcessing = true;

        //のけぞり処理
        Vector3 backwardForce = -transform.forward * knockbackForce_Back;
        Vector3 upwardForce = Vector3.up * knockbackForce_Up;
        rb.AddForce(backwardForce + upwardForce, ForceMode.Impulse);
        //

        goodSystem.AddGood(getGoodNum);
    }

    private void Die() //死亡処理
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        spawnManager.DefeatedEnemyCount++;
        Destroy(gameObject);
    }

    public bool IsGrounded() //地面に設置しているかどうかを判定する処理
    {
        Vector3 direction = Vector3.down * groundCheckDistance;
        Debug.DrawRay(transform.position, direction, Color.red);

        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}