using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//作成者：桑原

[System.Serializable]
class ComboStep
{
    [Header("攻撃判定用オブジェクト")]
    [SerializeField] public GameObject hitbox;
    [Header("命中時に発生するエフェクト")]
    [SerializeField] public GameObject hitEffect;
    [Header("命中時の音声")]
    public AudioClip hitSound;
    [Header("基本ダメージ")]
    [SerializeField] public float baseDamage = 10f;
    [Header("前方向への基本の吹き飛ばし力")]
    [SerializeField] public float baceForwardKnockbackForce = 1f;
    [Header("上方向への基本の吹き飛ばし力")]
    [SerializeField] public float baceUpwardKnockbackForce = 1f;
    [Header("攻撃判定の中心に引き寄せる力を有効にするか")]
    [SerializeField] public bool enableSuction = false;
    [Header("引き寄せる力")]
    [SerializeField] public float suctionForce = 10f;
    [Header("当たり判定の持続時間")]
    [SerializeField] public float attackDuration = 0.2f;
    [Header("攻撃判定消滅後、次の段に移行可能な猶予時間")]
    [SerializeField] public float comboResetTime = 1f;
    [Header("入力受け付後から攻撃までの時間")]
    [SerializeField] public float windupTime = 0.2f;
    [Header("攻撃時に移動する距離")]
    [SerializeField] public float attackMoveDistance = 1f;
   

}

public class CloseAttack : MonoBehaviour
{
    [Header("移動を制御するオブジェクト")]
    [SerializeField] Transform target;
    [Header("コンボ設定")]
    [SerializeField] List<ComboStep> comboSteps = new List<ComboStep>();
    [Header("攻撃時有効にするトレイルレンダラー")]
    [SerializeField] public List<TrailRenderer> renderers = new List<TrailRenderer>();
    [Header("必要なコンポーネント")]
    [SerializeField] PlayerStatus playerStatus;
    [SerializeField] DamageToTarget damageToTarget;
    [SerializeField] MovePlayer movePlayer;
    [SerializeField] Live2DController live2DController;
    [SerializeField]Live2DTalkPlayer live2DTalkPlayer;
    public enum AttackState { None, Windup, Attacking, Recovering }

    AttackState attackState = AttackState.None;

    int currentComboIndex = 0; //現在のコンボ段階を示す変数
    float lastAttackTime = 0f; //最後に攻撃した時間
    bool isAttackBuffered = false; //攻撃入力があったかどうか
    float stateTimer = 0f; //各状態の経過時間の計測用

    float movedDistance = 0f;
    float totalMoveDistance = 0f;

    public AttackState CurrentAttackState { get { return attackState; } private set { attackState = value; } }
    public int CurrentComboIndex { get { return currentComboIndex; } }

    public void TryAttack() //攻撃処理（近接攻撃ボタンを押したときに呼ばれる）
    {
        if (isAttackBuffered || currentComboIndex >= comboSteps.Count) return;

        isAttackBuffered = true;
        attackState = AttackState.Windup;

        ComboStep step = comboSteps[currentComboIndex];
        totalMoveDistance = step.attackMoveDistance;
        movedDistance = 0f;
        
        //Debug.Log(currentComboIndex + 1 + "段目");
    }

    public void CloseAttackProcess()
    {
        stateTimer += Time.deltaTime;

        HandleAttackMovement();

        switch (attackState)
        {
            case AttackState.Windup: //攻撃待機
                movePlayer.MoveSpeedMultiplier = 0f; //移動を制限
                if (stateTimer >= comboSteps[currentComboIndex].windupTime)
                    BeginAttack();
                break;

            case AttackState.Attacking: //攻撃中
                if (stateTimer >= comboSteps[currentComboIndex].attackDuration)
                    EndAttack();
                
                  
                break;

            case AttackState.Recovering: //攻撃後
                if (Time.time - lastAttackTime > GetCurrentComboResetTime())
                    ResetCombo();
                break;

            case AttackState.None:
                break;
        }
    }

    void BeginAttack() //攻撃開始時の処理
    {
        ComboStep step = comboSteps[currentComboIndex];

        damageToTarget.Damage = GetCurrentDamage(); //与えるダメージの代入
        damageToTarget.ForwardKnockbackForce = GetCurrentForwardForce(); //前方向へ吹き飛ばす力の代入
        damageToTarget.UpwardKnockbackForce = GetCurrentUpwardForce(); //上方向へ吹き飛ばす力の代入
        damageToTarget.HitEffect = comboSteps[currentComboIndex].hitEffect; //開始された攻撃の命中時エフェクトの設定
        damageToTarget.HitSound = comboSteps[currentComboIndex].hitSound; //開始された攻撃の命中時の音設定
        damageToTarget.EnableSuction = comboSteps[currentComboIndex].enableSuction; //引き寄せる力が有効かどうかの設定
        damageToTarget.SuctionForce = comboSteps[currentComboIndex].suctionForce; //引き寄せる力の代入

        if (step.hitbox != null) step.hitbox.SetActive(true); //攻撃判定の有効化

        movePlayer.RotationSpeedMultiplier = 0f; //プレイヤーの回転スピードの制御
        
        foreach(var trail in renderers)
        {
            trail.enabled = true;
        }
        stateTimer = 0f;
        attackState = AttackState.Attacking;

        Live2DPlay();

        //Debug.Log(currentComboIndex + 1 + "段目発生");
    }

    public void Live2DPlay()//Live2Dの攻撃モーションとセリフ再生
    {
        if(live2DController == null || live2DTalkPlayer == null) return;

        live2DTalkPlayer.PlayTalk("Attack_" + (currentComboIndex + 1).ToString());
        //Debug.Log("Attack_" + currentComboIndex + 1);
        if (currentComboIndex == comboSteps.Count - 1)//最終段の場合
        {
            live2DController.PlayMotion("Attack_High");
        }
        else
        {
            live2DController.PlayMotion("Attack_Low");
        }
    }
    void EndAttack() //発生した攻撃の終了処理
    {
        ComboStep step = comboSteps[currentComboIndex];

        if (step.hitbox != null) step.hitbox.SetActive(false); //攻撃判定の無効化

        movePlayer.RotationSpeedMultiplier = 1f;

        lastAttackTime = Time.time;      
        isAttackBuffered = false;
        stateTimer = 0f;
        attackState = AttackState.Recovering;

    //    foreach (var trail in renderers) { trail.enabled = false; }
        //Debug.Log(currentComboIndex + 1 + "段目終了");
        currentComboIndex++;
    }

    void ResetCombo() //コンボ段階の初期化
    {
        //各当たり判定の無効化
        foreach (var step in comboSteps)
        {
            if (step.hitbox != null) step.hitbox.SetActive(false);
            foreach (var trail in renderers) { 
            trail.enabled = false;
            
            }        
        
        }
           
      
        movePlayer.MoveSpeedMultiplier = 1f;
        isAttackBuffered = false;
        stateTimer = 0f;
        attackState = AttackState.None;        

        //Debug.Log(currentComboIndex + "コンボのリセット");
        currentComboIndex = 0;
        PlayerActionEvents.IdleEvent();
    }

    float GetCurrentComboResetTime() //次のコンボ段階までの猶予時間の取得
    {
        return comboSteps[currentComboIndex - 1].comboResetTime;
    }

    void HandleAttackMovement() //攻撃時の前進処理
    {
        if (attackState != AttackState.Windup || currentComboIndex >= comboSteps.Count) return;

        ComboStep step = comboSteps[currentComboIndex];

        float duration = step.windupTime; //windupTimeの間に前進を終える
        float movePerSecond = totalMoveDistance / duration;
        float moveDelta = movePerSecond * Time.deltaTime;

        float remaining = totalMoveDistance - movedDistance;
        float actualMove = Mathf.Min(moveDelta, remaining);

        target.position += target.forward.normalized * actualMove;
        movedDistance += actualMove;
    }

    float GetCurrentDamage() //現在の段のダメージ量を取得する
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baseDamage * attackPower; //最終的なダメージ量を返す
    }

    float GetCurrentForwardForce() //現在の攻撃の前方向への吹き飛ばし力を取得する
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baceForwardKnockbackForce * attackPower; //最終的な前方向への吹き飛ばし力を返す
    }

    float GetCurrentUpwardForce() //現在の攻撃の上方向への吹き飛ばし力を取得する
    {
        if (currentComboIndex >= comboSteps.Count || currentComboIndex < 0) return 0f;

        ComboStep step = comboSteps[currentComboIndex];
        float attackPower = playerStatus != null ? playerStatus.AttackPower : 1f;

        return step.baceUpwardKnockbackForce * attackPower; //最終的な前方向への吹き飛ばし力を返す
    }
}
