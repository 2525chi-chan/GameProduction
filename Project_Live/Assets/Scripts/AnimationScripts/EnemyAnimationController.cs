using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PlayIdle() //待機アニメーション
    {
        animator.SetTrigger("Idle");
    }

    public void  PlayMove() //移動アニメーション
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
}
