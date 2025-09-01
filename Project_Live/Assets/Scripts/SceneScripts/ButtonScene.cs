using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScene : MonoBehaviour//�V�[���J�ڂ�����{�^���ɃA�^�b�`
{
    [SerializeField]private SelectScene.SceneName sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var button = this.GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(()=>SelectScene.LoadScene(sceneName));
    }

    
}
