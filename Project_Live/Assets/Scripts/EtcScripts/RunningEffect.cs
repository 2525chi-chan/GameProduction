using UnityEngine;

public class RunningEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("�v���C���[�̃A�j���[�V����")]
    [SerializeField]Animator animator;
    [Header("���s�G�t�F�N�g")]
    [SerializeField] GameObject runningEffect;
    void Start()
    {
        if (runningEffect != null)
        {
            runningEffect.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runningEffect != null)
        {
            runningEffect.SetActive(animator.GetCurrentAnimatorStateInfo(0).IsName("Running"));
        }
    }
}
