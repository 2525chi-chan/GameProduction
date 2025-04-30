using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] MovePlayer movePlayer;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (playerAnimator != null)
            animator = playerAnimator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(movePlayer.Move, movePlayer.Prev_Move) > 0.01f)
            SetStateRunning();
    }

    void SetStateRunning()
    {
        animator.SetBool("Running", movePlayer.Move != Vector3.zero); //�ړ����Ȃ瑖�胂�[�V�������Z�b�g����
    }
}
