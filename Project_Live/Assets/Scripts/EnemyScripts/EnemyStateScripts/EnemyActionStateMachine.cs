using System.Collections.Generic;
using UnityEngine;

public class EnemyActionStateMachine : MonoBehaviour
{
    IEnemyState currentState; //現在の状態格納用

    [Header("生成後の待機時間設定")]
    [Tooltip("ここで設定した時間が経過するまで、何もしない")]
    [SerializeField] float initialDelayTime = 0f;
    [Header("群れとボスのどちらの行動パターンを適用するか")]
    [SerializeField] bool isBossBehaviour = false;

    //[Header("HP残量に応じて行動パターンの切り替えを適用するか")]
    //[SerializeField] bool isPatternChangeEnable = false;
    //[Header("ステート遷移を順番かランダムのどちらで行うか")]
    //[Tooltip("チェックをつけると順番、外すとランダムで状態が遷移する")]
    //[SerializeField] bool useSequentialOrder = true;

    public enum BossAttackType { Normal, LongRange, Area, MeteorFall }

    [System.Serializable]
    public class BossAttackPattern
    {
        [Header("この攻撃パターンに変化するHp残量")]
        [SerializeField] public float changeHp;

        [Header("通常攻撃を有効にするか")]
        public bool enableNormalAttack = true;
        [UnityEngine.Range(0f, 1f)]
        [Header("通常攻撃の発動しやすさ")]
        [Tooltip("数値が大きいほど発動確率が高くなる")]
        public float normalAttackWeight = 1f;
        
        [Header("遠距離攻撃を有効にするか")]
        public bool enableLongRangeAttack = true;
        [Header("遠距離攻撃の発動しやすさ")]
        [Tooltip("数値が大きいほど発動確率が高くなる")]
        [UnityEngine.Range(0f, 1f)]
        public float longRangeAttackWeight = 1f;
                
        [Header("円形範囲攻撃を有効にするか")]
        public bool enableAreaAttack = true;
        [Header("円形範囲攻撃の発動しやすさ")]
        [Tooltip("数値が大きいほど発動確率が高くなる")]
        [UnityEngine.Range(0f, 1f)]
        public float areaAttackWeight = 1f;

        [Header("隕石攻撃を有効にするか")]
        public bool enableMeteorFallAttack = true;
        [Header("隕石攻撃の発動しやすさ")]
        [Tooltip("数値が大きいほど発動確率が高くなる")]
        [UnityEngine.Range(0f, 1f)]
        public float meteorFallAttackWeight = 1f;
        //public List<BossAttackType> attackOrder = new List<BossAttackType>(); //順番指定用
    }
    [SerializeField] List<BossAttackPattern> attackPatterns =  new List<BossAttackPattern>();

    [Header("必要なコンポーネント")]
    [SerializeField] EnemyAnimationController anim;
    [SerializeField] EnemyStatus status;
    [SerializeField] EnemyMover mover;
    [SerializeField] AttackController attackController;
    [SerializeField] AttackTrigger attackTrigger;
    [SerializeField] NormalAttack_Boss normalAttack;
    [SerializeField] LongRangeAttack_Boss longRangeAttack;
    [SerializeField] WideAreaAttack_Boss wideAreaAttack;
    [SerializeField] MeteorFallAttack_Boss meteorFallAttack;

    public EnemyActionEvents actionEvents = new EnemyActionEvents();

    BossAttackPattern currentAttackPattern;

    bool isActive = false;
    float currentTimer = 0f;
    bool isDelayFinished;

    public bool IsActive { get { return isActive; } set { isActive = value; } }
    
    public IEnemyState CurrentState { get { return currentState; } }
    public bool IsBossBehaviour { get { return isBossBehaviour; } }

    void Awake()
    {
        currentTimer = 0f;
        isDelayFinished = false;

        status.actionEvents = actionEvents;
        mover.actionEvents = actionEvents;
        attackTrigger.actionEvents = actionEvents;
        ChangeState(new IdleState_Enemy(this, anim, status, mover));
    }

    void Start()
    {
        if (!isBossBehaviour) return;

        if (attackPatterns.Count > 0)
            currentAttackPattern = attackPatterns[0];

        OnBossAttackStartProcess();
    }

    void OnEnable()
    {
        actionEvents.OnIdleEvent += OnIdleProcess;
        actionEvents.OnMoveEvent += OnMoveProcess;
        actionEvents.OnKnockbackEvent += OnKnockbackProcess;
        actionEvents.OnDownEvent += OnDownProcess;
        actionEvents.OnAttackEvent += OnAttackProcess;
        actionEvents.OnBossAttackStartEvent += OnBossAttackStartProcess;
        actionEvents.OnBossAttackFinishEvent += OnBossAttackFinishProcess;
        actionEvents.OnNormalAttackEvent += OnNormalAttackProcess_Boss;
        actionEvents.OnLongRangeAttackEvent += OnLongRangeAttackProcess_Boss;
        actionEvents.OnAreaAttackEvent += OnAreaAttackProcess_Boss;
        actionEvents.OnMeteorFallAttackEvent += OnMeteorFallAttackProcess_Boss;
        actionEvents.OnLostEvent += OnLostProcess;
    }

    void OnDisable()
    {
        actionEvents.OnIdleEvent -= OnIdleProcess;
        actionEvents.OnMoveEvent -= OnMoveProcess;
        actionEvents.OnKnockbackEvent -= OnKnockbackProcess;
        actionEvents.OnDownEvent -= OnDownProcess;
        actionEvents.OnAttackEvent -= OnAttackProcess;
        actionEvents.OnBossAttackStartEvent -= OnBossAttackStartProcess;
        actionEvents.OnBossAttackFinishEvent -= OnBossAttackFinishProcess;
        actionEvents.OnNormalAttackEvent -= OnNormalAttackProcess_Boss;
        actionEvents.OnLongRangeAttackEvent -= OnLongRangeAttackProcess_Boss;
        actionEvents.OnAreaAttackEvent -= OnAreaAttackProcess_Boss;
        actionEvents.OnMeteorFallAttackEvent -= OnMeteorFallAttackProcess_Boss;
        actionEvents.OnLostEvent -= OnLostProcess;
    }

    IEnemyState GetNextBossAttack()
    {
        if (!isBossBehaviour || currentAttackPattern == null)
            return new IdleState_Enemy(this, anim, status, mover);

        BossAttackType nextAttack = GetRandomAttackByWeight(currentAttackPattern);
       
        return nextAttack switch
        {
            BossAttackType.Normal => new NormalAttackState_EnemyBoss(anim, mover, normalAttack),
            BossAttackType.LongRange => new LongRangeAttackState_EnemyBoss(anim, mover, longRangeAttack),
            BossAttackType.Area => new AreaAttackState_EnemyBoss(anim, mover, wideAreaAttack),
            BossAttackType.MeteorFall => new MeteorFallAttackState_EnemyBoss(anim, mover, meteorFallAttack),
            _ => new IdleState_Enemy(this, anim, status, mover)
        };
    }

    BossAttackType GetRandomAttackByWeight(BossAttackPattern pattern)
    {
        float totalWeight = 0f;

        if (pattern.enableNormalAttack && pattern.normalAttackWeight > 0f)
            totalWeight += pattern.normalAttackWeight;

        if (pattern.enableLongRangeAttack && pattern.longRangeAttackWeight > 0f)
            totalWeight += pattern.longRangeAttackWeight;

        if (pattern.enableAreaAttack && pattern.areaAttackWeight > 0f)
            totalWeight += pattern.areaAttackWeight;

        if (pattern.enableMeteorFallAttack && pattern.meteorFallAttackWeight > 0f)
            totalWeight += pattern.meteorFallAttackWeight;

        if (totalWeight <= 0f) return BossAttackType.Normal;

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0f;

        if (pattern.enableNormalAttack && pattern.normalAttackWeight > 0f)
        {
            cumulative += pattern.normalAttackWeight;
            if (randomValue <= cumulative)
                return BossAttackType.Normal;
        }

        if (pattern.enableLongRangeAttack && pattern.longRangeAttackWeight > 0f)
        {
            cumulative += pattern.longRangeAttackWeight;
            if (randomValue <= cumulative)
                return BossAttackType.LongRange;
        }

        if (pattern.enableAreaAttack && pattern.areaAttackWeight > 0f)
        {
            cumulative += pattern.areaAttackWeight;
            if (randomValue <= cumulative)
                return BossAttackType.Area;
        }

        if (pattern.enableMeteorFallAttack && pattern.meteorFallAttackWeight > 0f)
        {
            cumulative += pattern.meteorFallAttackWeight;
            if (randomValue <= cumulative)
                return BossAttackType.MeteorFall;
        }

        return BossAttackType.Normal;
    }

    BossAttackPattern GetPatternForCurrentHp()
    {
        if (attackPatterns == null || attackPatterns.Count == 0) return null;

        BossAttackPattern selected = attackPatterns[0];

        foreach (var pattern in attackPatterns)
        {
            if (status.Hp <= pattern.changeHp && pattern.changeHp <= selected.changeHp)
                selected = pattern;
        }

        return selected;
    }

    void OnBossAttackStartProcess()
    {
        if (!isBossBehaviour || (currentState is LostState_Enemy)) return;
        ChangeState(GetNextBossAttack());
    }

    void OnBossAttackFinishProcess()
    {
        if (!isBossBehaviour || (currentState is LostState_Enemy)) return;
        ChangeState(GetNextBossAttack());
    }

    void Update()
    {
        if (!isDelayFinished)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= initialDelayTime)
            {
                isDelayFinished = true;
                currentTimer = 0f;
            }
            return;
        }

        if (isBossBehaviour /*&& isPatternChangeEnable*/)
        {
            BossAttackPattern patternForHp = GetPatternForCurrentHp();

            if (patternForHp != null && patternForHp != currentAttackPattern)
            {
                currentAttackPattern = patternForHp;
                OnBossAttackStartProcess();
            }
        }

        if (status.Hp <= 0) actionEvents.LostEvent();

        currentState?.Update();
        //Debug.Log(currentState);
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void OnIdleProcess()
    {
        if (currentState is IdleState_Enemy) return;
        ChangeState(new IdleState_Enemy(this, anim, status, mover));
    }

    void OnMoveProcess()
    {
        if (currentState is IdleState_Enemy && !(currentState is MoveState_Enemy)) 
            ChangeState(new MoveState_Enemy(anim, mover));
    }

    void OnKnockbackProcess()
    {
        if (currentState is KnockbackState_Enemy || currentState is LostState_Enemy) return;
        ChangeState(new KnockbackState_Enemy(this, anim, status));
    }

    void OnDownProcess()
    {
        if (currentState is DownState_Enemy || currentState is LostState_Enemy) return;
        ChangeState(new DownState_Enemy(anim));
    }

    void OnAttackProcess()
    {
        if (currentState is AttackState_Enemy || currentState is LostState_Enemy) return;
        ChangeState(new AttackState_Enemy(anim, mover, attackController));
    }

    void OnNormalAttackProcess_Boss()
    {
        if (currentState is NormalAttackState_EnemyBoss || currentState is LostState_Enemy || currentState is KnockbackState_Enemy) return;
        ChangeState(new NormalAttackState_EnemyBoss(anim, mover, normalAttack));
    }

    void OnLongRangeAttackProcess_Boss()
    {
        if (currentState is LongRangeAttackState_EnemyBoss || currentState is LostState_Enemy || currentState is KnockbackState_Enemy) return;
        ChangeState(new LongRangeAttackState_EnemyBoss(anim, mover, longRangeAttack));
    }

    void OnAreaAttackProcess_Boss()
    {
        if (currentState is AreaAttackState_EnemyBoss || currentState is LostState_Enemy || currentState is KnockbackState_Enemy) return;
        ChangeState(new AreaAttackState_EnemyBoss(anim, mover, wideAreaAttack));
    }

    void OnMeteorFallAttackProcess_Boss()
    {
        if (currentState is MeteorFallAttackState_EnemyBoss || currentState is LostState_Enemy || currentState is KnockbackState_Enemy) return;
        ChangeState(new MeteorFallAttackState_EnemyBoss(anim, mover, meteorFallAttack));
    }

    void OnLostProcess()
    {
        if (currentState is LostState_Enemy) return;
        ChangeState(new LostState_Enemy(anim));
    }

    //void BuildBossAttackQueue(BossAttackQueue pattern)
    //{
    //    pattern.attackOrder.Clear();

    //    if (pattern.enableNormalAttack) pattern.attackOrder.Add(BossAttackType.Normal);
    //    if (pattern.enableLongRangeAttack) pattern.attackOrder.Add(BossAttackType.LongRange);
    //    if (pattern.enableAreaAttack) pattern.attackOrder.Add(BossAttackType.Area);

    //    if (!useSequentialOrder && pattern.attackOrder.Count > 1)
    //        ShuffleAttackOrder(pattern);
    //}

    //void ShuffleAttackOrder(BossAttackQueue pattern) //攻撃内容のシャッフル
    //{
    //    for (int i = pattern.attackOrder.Count - 1; i > 0; i--)
    //    {
    //        int randomIndex = Random.Range(0, 0 + i);
    //        var temp = pattern.attackOrder[i];
    //        pattern.attackOrder[i] = pattern.attackOrder[randomIndex];
    //        pattern.attackOrder[randomIndex] = temp;
    //    }
    //}
}
