using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class ParentClass
{
    [Header("バズリランク名")]
    [SerializeField] string name;

    [Header("このバズリランクに必要ないいね数")]
    [SerializeField] public float needNum;
    [Header("このバズリランクのいいね取得倍率")]
    [SerializeField] float goodMagnification;

    [Header("このランクで表示されるコメント")]
    [SerializeField] List<ChildClass> commentContents=new List<ChildClass>();

}
[System.Serializable]
public class ChildClass
{
    [Header("コメントの内容")]
    [SerializeField] string mainComment;
    [Header("関連コメントの内容")]
    [SerializeField] List<string> conectComment;
}


public class Demo : MonoBehaviour
{
    [SerializeField] List<ParentClass> rankList;

    int currrentIndex = 0;

    [System.NonSerialized]
    public ParentClass currentBuzzRank = new ParentClass();

    void Start()
    {
        currentBuzzRank = rankList[currrentIndex];
    }
}

