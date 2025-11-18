using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class AppearFans : MonoBehaviour
{
    [SerializeField]GoodSystem goodSystem;
    [SerializeField] BuzuriRank BuzuriRank;
    [SerializeField] int loadLimit;
    private List<(GameObject, MeshRenderer[])> fanObjects = new();
    private float loadCount = 0;
    private int goodLimit = 0;//ファンが最大数出るまでの数(現状神バズ到達時に最大になる)
    private int parFanAppear;//いいね何個でファンが一体出現するか
    private BuzzRank maxRank;
    private int apperedFan = 0;
    GameObject[] Fans;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxRank = BuzuriRank.BuzzRanks.Last();
        goodLimit = (int)maxRank.NeddNum;
       
        Fans = GameObject.FindGameObjectsWithTag("Fan");
       parFanAppear = goodLimit / Fans.Length;
       // Test();
       StartCoroutine(FanLoad(Fans));
        Debug.Log(parFanAppear);
    }

    public void Test()
    {
        for (int i = 0; i < Fans.Length; i++) {

            MeshRenderer[] renderers = Fans[i].GetComponentsInChildren<MeshRenderer>();


           
            foreach (var rend in renderers)
            {

                rend.enabled = true;
            }

        }
    }
    IEnumerator FanLoad(GameObject[] Fans)
    {

        foreach (var obj in Fans)
        {
            MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
           
            fanObjects.Add((obj, renderers));
            loadCount++;

         
            if (loadCount %loadLimit == 0)
            {
                yield return null;
            }

        }

    }
   private void Update()
    {
        while (goodSystem.DisplayGoodNum > parFanAppear * apperedFan+1)
        {
            var random = Random.Range(0, fanObjects.Count);
            var appearFan = fanObjects[random];
            fanObjects.Remove(appearFan);
            var renderers = appearFan.Item2;
            foreach(var rend in renderers)
            {

                rend.enabled = true;
            }
           
            apperedFan++;
        }
    }
   
}
