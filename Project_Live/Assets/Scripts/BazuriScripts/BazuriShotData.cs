using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class BazuriShotData:MonoBehaviour
{
 
    public new BazuriTag tag=new BazuriTag();
    public float score;
}
public�@enum BazuriTag
{
   Player,
   Enemy,
   Effect
}
