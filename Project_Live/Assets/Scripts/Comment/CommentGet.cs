//�쐬��:����

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentGet : MonoBehaviour
{
    GoodSystem goodSystem;
    bool getTrigger = false;    //2�d���h���p�̃t���O

    // Start is called before the first frame update
    void Start()
    {
        goodSystem= GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();    //�X�N���v�gGoodSystem�̕ϐ����������߂̂��܂��Ȃ�
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (getTrigger)
            return;
        if (other.gameObject.CompareTag("Player"))  //Player�Ɏ��ꂽ��s���鏈��  
        {
            getTrigger = true;
            Destroy(this.gameObject);
            goodSystem.GoodNum += 10f;
            Debug.Log("�R�����g���擾���܂���");
        }
    }
}
