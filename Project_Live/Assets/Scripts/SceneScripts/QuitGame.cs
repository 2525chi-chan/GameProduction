using UnityEngine;

public class QuitGame : MonoBehaviour//�Q�[���I���p�X�N���v�g
{
   

    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
