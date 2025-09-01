using UnityEngine;
using System.Collections;
public class GameOverManager : MonoBehaviour//�Q�[���I�[�o�[��ʑJ�ڂ��Ǘ�����
{
    [Header("���S��Q�[���I�[�o�[��ʂɑJ�ڂ���܂ł̎���")]
    [SerializeField]  float gameOverTime = 2f;

    public static GameOverManager Instance { get; private set; }

    void Awake() => Instance = this;
    public void StartGameOver()
    {
        StartCoroutine(GameOverInterval());
    }
    public  IEnumerator GameOverInterval()
    {
       
        yield return new WaitForSeconds(gameOverTime);
 Debug.Log("GameOver!!!!");
        SelectScene.LoadScene(SelectScene.SceneName.GameOver);

    }
}
