using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectStatusManager : MonoBehaviour
{
    [Header("管理するオブジェクトのタグ名")]
    [SerializeField] string targetTag = "Breakable";
    [Header("必要なコンポーネント")]
    [SerializeField] GameOverManager gameOverManager;
    [SerializeField]ObjectStatusEffector objectStatusEffector;
    List<ObjectStatus> objectStatuses = new List<ObjectStatus>();

    public List<ObjectStatus> ObjectStatuses { get { return objectStatuses; } }
    bool isGameOverCalled = false;

    void Start()
    {
        RegisterObjects();
    }

    void Update()
    {
        CheckAllBroken();
    }

    void RegisterObjects() //シーン上に存在する、特定のタグを持つオブジェクトの登録を行う
    {
        objectStatuses.Clear();

        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (var obj in targets)
        {
            ObjectStatus status = obj.GetComponent<ObjectStatus>();

            if (status != null)
                objectStatuses.Add(status);
            objectStatusEffector.MaxHpSum += (int)status.MaxHp;
            
        }
    }

    void CheckAllBroken() //全てのオブジェクトが破壊されたか調べる
    {
        if (isGameOverCalled) return;

        foreach (var status in objectStatuses)
        {
            if (status != null && !status.IsBroken)
                return;
        }

        isGameOverCalled = true;
        OnAllObjectsBroken();
    }

    void OnAllObjectsBroken() //全てのオブジェクトが破壊されたときの処理
    {
        Debug.Log("全ての配信機材が破壊されました");
        gameOverManager.StartGameOver();    }
}
