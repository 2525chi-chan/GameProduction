using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    void OnDisable()
    {
        UnlockAnyState();
    }

    public void PlayIdle() //待機アニメーション
    {
        animator.SetTrigger("Idle");
    }

    public void PlayMove() //移動アニメーション
    {
        animator.SetTrigger("Idle");
    }

    public void PlayDamage() //のけぞりアニメーション
    {
        animator.SetTrigger("Damage");
    }

    public void PlayDown() //倒れアニメーション
    {
        animator.SetTrigger("Down");
    }

    public void PlayAttack() //近接攻撃アニメーション
    {
        animator.SetTrigger("Attack");
    }

    public void PlayNormalAttack() //近接攻撃アニメーション
    {
        animator.SetTrigger("NormalAttack");
    }

    public void PlayLongRangeAttack() //遠距離攻撃アニメーション
    {
        animator.SetTrigger("LongRangeAttack");
    }

    public void PlayAreaAttack() //広範囲攻撃アニメーション
    {
        animator.SetTrigger("AreaAttack");
    }

    public void PlayMeteorFallAttack() //隕石攻撃アニメーション
    {
        animator.SetTrigger("MeteorFallAttack");
    }

    public void PlayLost() //撃破時アニメーション
    {
        animator.SetTrigger("Lost");
    }

    public void LockAnyState()
    {
        animator.SetBool("AnyStateLock", true);
    }

    public void UnlockAnyState()
    {
        animator.SetBool("AnyStateLock", false);
    }
}
