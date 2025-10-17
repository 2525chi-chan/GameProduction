using UnityEngine;
using  System.Collections.Generic;
using System.Linq;
public class EnemyRagdoll : MonoBehaviour
{

    public CharacterJoint leftJoint;
    public CharacterJoint rightJoint;
    public Rigidbody baseJointRigid;//本体にくっつけるRigidbody
    GameObject enemy;
    public List <Rigidbody> rigidbodies;
    List <Transform>defaultPos = new List<Transform>();
    EnemyStatus status;
    Animator animator;
    EnemyMover mover;
    EnemyMoveState moveState;
    Rigidbody baseRigid;//本体のRigidbody

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = this.gameObject;
        status= enemy.GetComponent<EnemyStatus>();
        baseRigid = enemy.GetComponent<Rigidbody>();
        mover =enemy.GetComponent<EnemyMover>();
        rigidbodies = new List<Rigidbody>(enemy.GetComponentsInChildren<Rigidbody>());

        rigidbodies.RemoveAll(rb => rb.name == enemy.name);
      animator=this.GetComponent<Animator>();
    }
    private void Update()
    {
      
        
    }

    public void SwitchRagdoll(bool isRagdol)//ラグドール状態の切り替え。trueでラグドール状態、falseで通常状態
    {
        if(animator!=null)animator.enabled = !isRagdol;
        status.IsRagdoll = isRagdol;

        if (animator.enabled)
        {
            animator.SetTrigger("Idle");
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic =!isRagdol;
            
            
        }

        if (isRagdol&&leftJoint!=null&&rightJoint!=null)
        {

            enemy.AddComponent<CharacterJoint>();
            CharacterJoint joint=enemy.GetComponent<CharacterJoint>();
          joint.connectedBody = baseJointRigid;
           leftJoint.connectedBody = baseRigid;
            rightJoint.connectedBody = baseRigid;

            moveState = mover.MoveState;//止まる前の状態を保持
            mover.MoveSetState(EnemyMoveState.stop);
            Debug.Log("とまる");
        }
        else
        {
            mover.MoveSetState(moveState);
            if (leftJoint != null && rightJoint != null)
            {
                leftJoint.connectedBody = null;
                rightJoint.connectedBody = null;
            }
                
            Destroy(enemy.GetComponent<CharacterJoint>());
        }
     
    }

}
