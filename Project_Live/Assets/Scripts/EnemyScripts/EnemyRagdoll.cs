using UnityEngine;
using  System.Collections.Generic;
public class EnemyRagdoll : MonoBehaviour
{

    GameObject enemy;
    public List <Rigidbody> rigidbodies;
   
   Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = this.gameObject;
        rigidbodies = new List<Rigidbody>(enemy.GetComponentsInChildren<Rigidbody>());
        rigidbodies.RemoveAll(rb => rb.name == enemy.name);
      animator=this.GetComponent<Animator>();
    }

    public void SwitchRagdoll(bool isRagdol,Vector3 dir)//���O�h�[����Ԃ̐؂�ւ��Bfalse�Ń��O�h�[����ԁAtrue�Œʏ���
    {
        if(animator!=null)animator.enabled = isRagdol;

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic =isRagdol;
            if(rb.name=="Spine1")
            {
                rb.AddForce(dir,ForceMode.Impulse);
            }
            
        }

      
        Debug.Log("aaaa");
    }

}
