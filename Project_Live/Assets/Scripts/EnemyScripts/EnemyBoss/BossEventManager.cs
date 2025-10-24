using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class BossEventManager : MonoBehaviour//�{�X�o��C�x���g�̊Ǘ�
{
    [SerializeField] FadeManager fadeManager;//�t�F�[�h
    [SerializeField]BossSpawnManager bossSpawnManager;//�{�X�����}�l�[�W���[
    [SerializeField] PlayerInput playerInput;
    [SerializeField]float fadeWaitTime = 1f;

    GameObject player;
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public IEnumerator BossEvent()
    {
        //�{�X�o��C�x���g
        playerInput.SwitchCurrentActionMap("Movie");
        yield return StartCoroutine(fadeManager.FadeIn());
        player.transform.position = new(0, 2, 0);//�v���C���[�̈ʒu���Z�b�g
        yield return StartCoroutine(fadeManager.FadeOut());
        bossSpawnManager.SpawnBoss();


        yield return new WaitForSeconds(fadeWaitTime);

        //�Q�[���؂�ւ�
        yield return StartCoroutine(fadeManager.FadeIn());
 yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());
       


        playerInput.SwitchCurrentActionMap("Player");
    }
}
