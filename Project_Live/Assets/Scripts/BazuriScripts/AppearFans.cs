using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
public class AppearFans : MonoBehaviour
{
    [SerializeField]GoodSystem goodSystem;
    [SerializeField] BuzuriRank BuzuriRank;
    [SerializeField] int loadLimit;
    [SerializeField] int appearLimit=5;
    private List<(GameObject, MeshRenderer[])> fanObjects = new();

    private Queue<(GameObject, MeshRenderer[])> fanQueue;
    private float loadCount = 0;
    private int goodLimit = 0;//ファンが最大数出るまでの数(現状神バズ到達時に最大になる)
    private int parFanAppear;//いいね何個でファンが一体出現するか
    private BuzzRank maxRank;
    private int appearedFan = 0;

    private int appeareCount = 0;

    private bool loaded = false;
    GameObject[] Fans;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


     void Shuffle<T>(IList<T>list)
    {
      System.Random rand = new System.Random();
        int num = list.Count;

        while (num > 0)
        {
            num--;
          int k=  rand.Next(num+1);

            T value = list[k];  
            list[k] = list[num];

            list[num] = value;

        }



    }
    void Start()
    {
        maxRank = BuzuriRank.BuzzRanks.Last();
        goodLimit = (int)maxRank.NeddNum;
       
        Fans = GameObject.FindGameObjectsWithTag("Fan");
       parFanAppear = goodLimit / Fans.Length;
   
       StartCoroutine(FanLoad(Fans));
        Debug.Log(parFanAppear);
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
        Shuffle(fanObjects);
        fanQueue = new Queue<(GameObject, MeshRenderer[])>(fanObjects);
        loaded = true;
    }
   private void Update()
    {
        appeareCount = 0;
        while (goodSystem.DisplayGoodNum > parFanAppear * appearedFan&&fanQueue.Count>0
            &&appeareCount<appearLimit&&loaded)
        {
          
            var appearFan = fanQueue.Dequeue();
         
            FanMove swing = appearFan.Item1.GetComponent<FanMove>();
            var renderers = appearFan.Item2;

            if (swing != null)
            {
                swing.SwingStart();
            }
            foreach(var rend in renderers)
            {

                rend.enabled = true;
            }
           
            appearedFan++;

            appeareCount++;
        }
    }
   
}
