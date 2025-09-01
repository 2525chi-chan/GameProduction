using UnityEngine;

public class QuitGame : MonoBehaviour//ゲーム終了用スクリプト
{
   

    public void EndGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
