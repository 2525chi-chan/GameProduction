using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void PlayIdle() //待機アニメーション
    {
        ResetAllParameters();
    }

    public void  PlayMove() //移動アニメーション
    {

    }

    public void PlayKnockback() //のけぞりアニメーション
    {

    }

    public void PlayDown() //倒れアニメーション
    {

    }

    public void PlayCloseAttack() //近接攻撃アニメーション
    {

    }

    public void PlayShot() //遠距離攻撃アニメーション
    {

    }

    public void ResetAllParameters() //アニメーションコントローラー用変数の初期化
    {

    }
}
