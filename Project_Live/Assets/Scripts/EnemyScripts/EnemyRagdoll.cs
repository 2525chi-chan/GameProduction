using UnityEngine;
using  System.Collections.Generic;
public class EnemyRagdoll : MonoBehaviour
{

    GameObject enemy;
    public List <Rigidbody> rigidbodies;
    EnemyStatus status;
    Animator animator;
    EnemyMover mover;
    EnemyMoveState moveState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = this.gameObject;
        status= enemy.GetComponent<EnemyStatus>();
        mover=enemy.GetComponent<EnemyMover>();
        rigidbodies = new List<Rigidbody>(enemy.GetComponentsInChildren<Rigidbody>());
        rigidbodies.RemoveAll(rb => rb.name == enemy.name);
      animator=this.GetComponent<Animator>();
    }

    public void SwitchRagdoll(bool isRagdol)//ラグドール状態の切り替え。falseでラグドール状態、trueで通常状態
    {
        if(animator!=null)animator.enabled = isRagdol;
        status.IsRagdoll = isRagdol;

        if (animator.enabled)
        {
            animator.SetTrigger("Idle");
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic =isRagdol;
            
        }

        if (!isRagdol)
        {
            moveState = mover.MoveState;//止まる前の状態を保持
            mover.MoveSetState(EnemyMoveState.stop);
        }
        else
        {
            mover.MoveSetState(moveState);
        }
     
    }

}
