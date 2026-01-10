using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

//作成者   寺村

public class BuffInfo
{
    public float originalValue;
    public float buffedValue;
    public float timer = 0f;
    public float duration;
    public bool isActive = false;

    public BuffInfo(float original, float buffed, float duration)
    {
        this.originalValue = original;
        this.buffedValue = buffed;
        this.duration = duration;
        this.isActive = true;
    }
}


public class PlayerBuffManager : MonoBehaviour
{
    [Header("パワーバフ出現時のエフェクト")]
    [SerializeField] ParticleSystem powerBuffEffect;
    [Header("スピードバフ出現時のエフェクト")]
    [SerializeField] ParticleSystem agilityBuffEffect;
    [Header("HP回復の音声")]
    [SerializeField] AudioClip healSound;

    [Header("必要なコンポーネント")]
    [SerializeField] GameObject AgilityImage;
    [SerializeField] TextMeshProUGUI AgilityTimerText;
    [SerializeField] GameObject PowerImage;
    [SerializeField] TextMeshProUGUI PowerTimerText;
    [SerializeField] AudioSource SE;

    PlayerStatus playerStatus;

    float originalHp;

    public BuffInfo attackBuff;
    public BuffInfo speedBuff;

    void Start()
    {
        playerStatus = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();

        AgilityImage.SetActive(false);
        AgilityTimerText.enabled = false;

        PowerImage.SetActive(false);
        PowerTimerText.enabled = false;

        originalHp=playerStatus.Hp;
    }

    void Update()
    {
        // 攻撃力バフ処理
        if (attackBuff != null && attackBuff.isActive)
        {
            attackBuff.timer += Time.deltaTime;
            int remainingTime = (int)(attackBuff.duration - attackBuff.timer);
            PowerTimerText.text=remainingTime.ToString();

            if (attackBuff.timer >= attackBuff.duration)
            {
                playerStatus.AttackPower = attackBuff.originalValue;
                PowerImage.SetActive(false);
                PowerTimerText.enabled=false;
                Debug.Log("攻撃力のバフが切れました。現在の攻撃力は" + playerStatus.AttackPower + "です。");
                attackBuff.isActive = false;
                attackBuff = null;
            }
        }

        // 移動速度バフ処理
        if (speedBuff != null && speedBuff.isActive)
        {
            speedBuff.timer += Time.deltaTime;
            int remainingTime=(int)(speedBuff.duration-speedBuff.timer);
            AgilityTimerText.text=remainingTime.ToString();

            if (speedBuff.timer >= speedBuff.duration)
            {
                playerStatus.Agility = speedBuff.originalValue;
                AgilityImage.SetActive(false);
                AgilityTimerText.enabled=false;
                Debug.Log("素早さのバフが切れました。現在の素早さは" + playerStatus.Agility + "です。");
                speedBuff.isActive = false;
                speedBuff = null;
            }
        }
    }

    public void AddHP(float value)
    {
        int addValue = (int)(originalHp * (value / 100));
        SE.PlayOneShot(healSound);
        Debug.Log("プレイヤーの最大HP" + originalHp + "の" + value + "%分(" + addValue + ")プレイヤーのHPを回復しました。");
        playerStatus.Hp += addValue;
    }


    // 攻撃力にバフをかける
    public void BuffAttack(float magnification, float duration)
    {
        if (attackBuff != null && attackBuff.isActive)
        {
            attackBuff.timer = 0f;
        }
        else
        {
            PowerImage.SetActive(true);
            powerBuffEffect.Play();
            PowerTimerText.enabled = true;
            float original = playerStatus.AttackPower;
            float buffed = original * magnification;
            playerStatus.AttackPower = buffed;
            attackBuff = new BuffInfo(original, buffed, duration);
            Debug.Log("攻撃力が" + magnification + "倍されました。現在の攻撃力は" + playerStatus.AttackPower + "です。");
        }
    }

    // 移動速度にバフをかける
    public void BuffMoveSpeed(float magnification, float duration)
    {
        if (speedBuff != null && speedBuff.isActive)
        {
            speedBuff.timer = 0f;
        }
        else
        {
            AgilityImage.SetActive(true);
            agilityBuffEffect.Play();
            AgilityTimerText.enabled = true;
            float original = playerStatus.Agility;
            float buffed = original * magnification;
            playerStatus.Agility = buffed;
            speedBuff = new BuffInfo(original, buffed, duration);
            Debug.Log("素早さが" + magnification + "倍されました。現在の素早さは" + playerStatus.Agility + "です。");
        }
    }
}
