using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class GameViewRefresh : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 static  GameViewRefresh()
    {
        EditorApplication.update += AutoRefreshGameView;
    }

   static void AutoRefreshGameView()
    {
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.Repaint();
        }
    }
}
