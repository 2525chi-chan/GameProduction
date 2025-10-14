using UnityEngine;
using  System.Collections.Generic;
public class EnemyRagdoll : MonoBehaviour
{
   
    public List<CharacterJoint> Shoulders=new List<CharacterJoint>();
    GameObject enemy;
    public List <Rigidbody> rigidbodies;
    List <Transform>defaultPos = new List<Transform>();
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
    private void Update()
    {
      
        
    }

    public void SwitchRagdoll(bool isRagdol,Vector3 dir)//���O�h�[����Ԃ̐؂�ւ��Btrue�Ń��O�h�[����ԁAfalse�Œʏ���
    {
      //  if(animator!=null)animator.enabled = !isRagdol;
        status.IsRagdoll = isRagdol;

        if (animator.enabled)
        {
           // animator.SetTrigger("Idle");
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic =!isRagdol;
            
            
        }

        if (isRagdol)
        {

            enemy.AddComponent<CharacterJoint>();
            CharacterJoint joint=enemy.GetComponent<CharacterJoint>();
            joint.connectedBody = mainRigid;
            moveState = mover.MoveState;//�~�܂�O�̏�Ԃ�ێ�
            mover.MoveSetState(EnemyMoveState.stop);
            Debug.Log("�Ƃ܂�");
        }
        else
        {
            mover.MoveSetState(moveState);
        }
     
    }

}
