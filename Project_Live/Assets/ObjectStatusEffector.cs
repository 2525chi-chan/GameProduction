using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ObjectStatusEffector : MonoBehaviour
{
    [SerializeField]List<Image> effectedUI=new List<Image>();
    public  Material effectMaterial;
    ObjectStatusManager objectStatusManager;

    public int currentHpSum;

    private int maxHpSum;
    public int MaxHpSum
    {
        get { return maxHpSum; }
        set { maxHpSum = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectStatusManager = GetComponent<ObjectStatusManager>();
        foreach(var ui in effectedUI)
        {
            ui.material = effectMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHpSum = 0;
        foreach(var obj in objectStatusManager.ObjectStatuses)
        {
            currentHpSum += (int)obj.Hp;
        }
        var ratio = (float)currentHpSum / maxHpSum;


        foreach(var ui in effectedUI)
        {
           
            ui.material.SetFloat("_GrayscaleAmount", 1-ratio);
        }
    }
}
