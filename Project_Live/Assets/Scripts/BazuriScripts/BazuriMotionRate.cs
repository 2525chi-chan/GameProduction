using UnityEngine;
using System.Collections.Generic;
using System.Linq;  // ★追加

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

[System.Serializable]
public class Bazuri
{
    public string motionName;
    public float motionRate = 1f;
}

public class BazuriMotionRate : MonoBehaviour
{
    [SerializeField] List<Bazuri> bazuriMotion = new();
    [SerializeField] Animator animator;

    List<string> nameList = new List<string>();
    bool isInitialized = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        InitializeMotionListEditor();
    }
#endif

    void Start()
    {
        animator = GetComponent<Animator>();
#if UNITY_EDITOR
        InitializeMotionListEditor();
#else
        // ★ビルド時: AnimationClipからState名取得（高速・安全）
        InitializeMotionListRuntime();
#endif
        isInitialized = true;
    }

    // Editor専用: AnimatorController解析
#if UNITY_EDITOR
    void InitializeMotionListEditor()
    {
        if (animator?.runtimeAnimatorController == null) return;
        var ac = animator.runtimeAnimatorController as AnimatorController;
        if (ac?.layers == null || ac.layers.Length == 0) return;

        nameList.Clear();
        GetAllStateMachineName(ac.layers[0].stateMachine, nameList);

        UpdateBazuriList();
    }
#endif

    // ビルド時: AnimationClip名を使用（State名とほぼ一致）
    void InitializeMotionListRuntime()
    {
        if (animator?.runtimeAnimatorController == null) return;

        nameList.Clear();
        var clips = animator.runtimeAnimatorController.animationClips;
        if (clips != null)
        {
            foreach (var clip in clips)
                nameList.Add(clip.name);
        }

        UpdateBazuriList();
    }

    void UpdateBazuriList()
    {
        if (nameList.Count == 0) return;

        Dictionary<string, float> prevRate = bazuriMotion
            .Where(b => !string.IsNullOrEmpty(b.motionName))
            .ToDictionary(b => b.motionName, b => b.motionRate);

        bazuriMotion.Clear();
        foreach (var name in nameList.Distinct())
        {
            bazuriMotion.Add(new Bazuri
            {
                motionName = name,
                motionRate = prevRate.ContainsKey(name) ? prevRate[name] : 1f
            });
        }
    }

    public float GetCurrentMotionRate(Animator targetAnimator = null)
    {
        targetAnimator ??= animator;
        if (targetAnimator == null) return 1f;

        var stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
        foreach (var bazuri in bazuriMotion)
        {
            if (stateInfo.IsName(bazuri.motionName))
                return bazuri.motionRate;
        }
        return 1f;
    }

#if UNITY_EDITOR
    private static void GetAllStateMachineName(AnimatorStateMachine stateMachine, List<string> result)
    {
        if (stateMachine?.states == null) return;
        foreach (var state in stateMachine.states)
        {
            if (state.state != null) result.Add(state.state.name);
        }

        if (stateMachine.stateMachines != null)
        {
            foreach (var sub in stateMachine.stateMachines)
                GetAllStateMachineName(sub.stateMachine, result);
        }
    }
#endif
}
