using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }
    public void PlayIdle() //待機アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("TransitionNo", 0);
    }

    public void PlayMove() //移動アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("TransitionNo", 1);
    }

    public void PlayCloseAttack1() //近接攻撃1アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("CloseAttackNo", 0);
    }

    public void PlayCloseAttack2() //近接攻撃2アニメーションの設定
    {
        animator.SetInteger("CloseAttackNo", 1);
    }

    public void PlayCloseAttack3() //近接攻撃3アニメーションの設定
    {
        animator.SetInteger("CloseAttackNo", 2);
    }

    public void PlayCloseAttack4() //近接攻撃4アニメーションの設定
    {
        animator.SetInteger("CloseAttackNo", 3);
    }

    public void PlayShotAttack() //遠距離攻撃アニメーションの設定
    {
        ResetAllParameters();
        animator.SetTrigger("Shot");
    }

    public void PlayDodge() //回避アニメーションの設定
    {
        ResetAllParameters();
        animator.SetTrigger("Dodge");
    }

    public void PlayGoodAction1() //いいねアクション1アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("GoodActionNo", 0);
    }

    public void PlayGoodAction2() //いいねアクション2アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("GoodActionNo", 1);
    }

    public void PlayGoodAction3() //いいねアクション3アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("GoodActionNo", 2);
    }

    public void PlayGoodAction4() //いいねアクション4アニメーションの設定
    {
        ResetAllParameters();
        animator.SetInteger("GoodActionNo", 3);
    }

    public void PlayEmote1() //エモート1の再生
    {
        ResetAllParameters();
        animator.SetInteger("EmoteNo", 0);
    }

    public void PlayEmote2() //エモート2の再生
    {
        ResetAllParameters();
        animator.SetInteger("EmoteNo", 1);
    }

    public void PlayEmote3() //エモート3の再生
    {
        ResetAllParameters();
        animator.SetInteger("EmoteNo", 2);
    }

    public void PlayEmote4() //エモート4の再生
    {
        ResetAllParameters();
        animator.SetInteger("EmoteNo", 3);
    }

    public void ResetAllParameters() //アニメーションの遷移用パラメータのリセット
    {
        animator.SetInteger("TransitionNo", -1);
        animator.SetInteger("CloseAttackNo", -1);
        animator.ResetTrigger("Shot");
        animator.ResetTrigger("Dodge");
        animator.SetInteger("GoodActionNo", -1);
        animator.SetInteger("EmoteNo", -1);
    }
}
