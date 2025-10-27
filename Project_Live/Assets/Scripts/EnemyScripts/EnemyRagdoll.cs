using UnityEngine;
using  System.Collections.Generic;
using System.Linq;
using System.Collections;
public class EnemyRagdoll : MonoBehaviour//���O�h�[������
{

    public CharacterJoint leftJoint;
    public CharacterJoint rightJoint;
    public Rigidbody baseJointRigid;//�{�̂ɂ�������Rigidbody
    public float restoreDuration = 0.2f;//���O�h�[���������Ɍ��̈ʒu�ɖ߂�܂ł̎���
    public float restoreAnimationSpeed = 0.5f;
    public bool enableRagdoll = true;//���O�h�[����L���ɂ��邩�ǂ���
    List <Rigidbody> rigidbodies;
    GameObject enemy;
   
  
    List<Vector3>defaultTrans = new List<Vector3>();
    List<Quaternion> defaultRote = new List<Quaternion>();
    EnemyStatus status;
    Animator animator;
    EnemyMover mover;
    EnemyMoveState moveState;
    Rigidbody baseRigid;//�{�̂�Rigidbody

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = this.gameObject;
        status = enemy.GetComponent<EnemyStatus>();
        baseRigid = enemy.GetComponent<Rigidbody>();
        mover = enemy.GetComponent<EnemyMover>();
        rigidbodies = new List<Rigidbody>(enemy.GetComponentsInChildren<Rigidbody>());  
        
        rigidbodies.RemoveAll(rb => rb.name == enemy.name);
        foreach (Rigidbody rb in rigidbodies)
        {
            defaultTrans.Add(rb.transform.localPosition);
            defaultRote.Add(rb.transform.localRotation);
        }
      
      
        animator = this.GetComponent<Animator>();
    }
   

    public void SwitchRagdoll(bool isRagdol)//���O�h�[����Ԃ̐؂�ւ��Btrue�Ń��O�h�[����ԁAfalse�Œʏ���
    {
       if (!enableRagdoll) return;//���O�h�[���������͏������Ȃ�
        if (animator!=null)animator.enabled = !isRagdol;
        status.IsRagdoll = isRagdol;

        if (animator.enabled)
        {
            animator.SetTrigger("Idle");
        }

        for (int i=0;  i<rigidbodies.Count;i++)
        {
            var rb = rigidbodies[i];
           
            rb.isKinematic =!isRagdol;
            if (!isRagdol)
            {
                StartCoroutine(SmoothRestoreRagdoll(rb.transform, restoreDuration, defaultTrans[i], defaultRote[i]));
            }
                
        }

        if (isRagdol&&leftJoint!=null&&rightJoint!=null)//���O�h�[���ɂ���
        {

            enemy.AddComponent<CharacterJoint>();
            CharacterJoint joint=enemy.GetComponent<CharacterJoint>();
          joint.connectedBody = baseJointRigid;
           leftJoint.connectedBody = baseRigid;
            rightJoint.connectedBody = baseRigid;

            moveState = mover.MoveState;//�~�܂�O�̏�Ԃ�ێ�
            mover.MoveSetState(EnemyMoveState.stop);
            Debug.Log("�Ƃ܂�");
        }
        else//�ʏ��Ԃɖ߂�
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

    public IEnumerator SmoothRestoreRagdoll(Transform target, float duration, Vector3 defaultPos, Quaternion defaultRote)
    {
        float timeCount = 0f;
        Vector3 startPos = target.localPosition;
        Quaternion startRote = target.localRotation;
        animator.speed = restoreAnimationSpeed;
        while (timeCount < duration)
        {
            float t = timeCount / duration;

            target.localPosition = Vector3.Lerp(startPos, defaultPos, t);
            target.localRotation = Quaternion.Lerp(startRote, defaultRote, t);
            animator.speed=Mathf.Lerp(restoreAnimationSpeed, 1f, t);
            timeCount += Time.deltaTime;
            yield return null;
        }
        target.localPosition = defaultPos;
        target.localRotation = defaultRote;
        animator.speed = 1f;
    }
}
