using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PlayIdle() //待機アニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Idle");
    }

    public void  PlayMove() //移動アニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Idle");
    }

    public void PlayDamage() //のけぞりアニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Damage");
    }

    public void PlayDown() //倒れアニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Down");
    }

    public void PlayCloseAttack() //近接攻撃アニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Attack");
    }

    public void PlayShot() //遠距離攻撃アニメーション
    {
        ResetAllParameters();
        animator.SetTrigger("Idle");
    }

    public void ResetAllParameters() //アニメーションコントローラー用変数の初期化
    {
        //animator.SetBool("Idle", false);
        //animator.SetBool("Move", false);
        //animator.SetBool("Damage", false);
        //animator.SetBool("Attack", false);
        //animator.SetBool("Shot", false);
    }
}
