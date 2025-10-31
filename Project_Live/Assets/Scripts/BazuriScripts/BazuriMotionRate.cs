using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

[System.Serializable]
 public class Bazuri
    {
     public   string motionName;
       public float motionRate=1f;
    }


public class BazuriMotionRate : MonoBehaviour
{
  
    [SerializeField] List<Bazuri> bazuriMotion;
    Animator animator;
 
    List<string> nameList=new List<string>();
    public List<Bazuri> BazuriMotion
    {
        get { return bazuriMotion; }
        set { bazuriMotion = value; }
    }
    private void Reset()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()//スクリプト/インスペクター更新時に自動で呼び出される
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
           
        }
         var ac = animator.runtimeAnimatorController as AnimatorController;
        if (ac == null) return;

        nameList.Clear();
        GetAllStateMachineName(ac.layers[0].stateMachine, nameList);

        Dictionary<string, float> prevRate = new();
        
        foreach(var bazuri in bazuriMotion)//直前のデータの保存
        {

            if (!string.IsNullOrEmpty(bazuri.motionName))
            {
                prevRate[bazuri.motionName]=bazuri.motionRate;
            }
        }

        bazuriMotion.Clear();
       
       
       
        
        for(int i = 0; i < nameList.Count; i++)
        {
            var bazuri = new Bazuri
            {
                motionName = nameList[i],
                motionRate = prevRate.ContainsKey(nameList[i]) ? prevRate[nameList[i]] :1f
            };


            bazuriMotion.Add(bazuri);
        }
    }
  #endif
    public float GetCurrentMotionRate()//今のモーションのスコア倍率を返す
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        foreach (var bazuri in BazuriMotion)
        {
            if (stateInfo.IsName(bazuri.motionName))
            {
                return bazuri.motionRate;
            }
            
        }
        return 1f;
    }

    private static void GetAllStateMachineName(AnimatorStateMachine stateMachine,List<string > result)
    {
        foreach(var state in stateMachine.states)
        {
            result.Add(state.state.name);
        }

        foreach (var substateMachines in stateMachine.stateMachines)
        {
            GetAllStateMachineName(substateMachines.stateMachine, result);
        }
    }

}
