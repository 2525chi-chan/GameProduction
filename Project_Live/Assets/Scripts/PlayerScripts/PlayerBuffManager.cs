using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

//�쐬��   ����

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
    [Header("�K�v�ȃR���|�[�l���g")]
    [SerializeField] GameObject AgilityImage;
    [SerializeField] TextMeshProUGUI AgilityTimerText;
    [SerializeField] GameObject PowerImage;
    [SerializeField] TextMeshProUGUI PowerTimerText;

    PlayerStatus playerStatus;

    public BuffInfo attackBuff;
    public BuffInfo speedBuff;

    void Start()
    {
        playerStatus = GameObject.FindWithTag("PlayerStatus").GetComponent<PlayerStatus>();

        AgilityImage.SetActive(false);
        AgilityTimerText.enabled = false;

        PowerImage.SetActive(false);
        PowerTimerText.enabled = false;
    }

    void Update()
    {
        // �U���̓o�t����
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
                Debug.Log("�U���͂̃o�t���؂�܂����B���݂̍U���͂�" + playerStatus.AttackPower + "�ł��B");
                attackBuff.isActive = false;
                attackBuff = null;
            }
        }

        // �ړ����x�o�t����
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
                Debug.Log("�f�����̃o�t���؂�܂����B���݂̑f������" + playerStatus.Agility + "�ł��B");
                speedBuff.isActive = false;
                speedBuff = null;
            }
        }
    }

    public void AddHP(float value)
    {
        playerStatus.Hp += value;
    }


    // �U���͂Ƀo�t��������
    public void BuffAttack(float magnification, float duration)
    {
        if (attackBuff != null && attackBuff.isActive)
        {
            attackBuff.timer = 0f;
        }
        else
        {
            PowerImage.SetActive(true);
            PowerTimerText.enabled = true;
            float original = playerStatus.AttackPower;
            float buffed = original * magnification;
            playerStatus.AttackPower = buffed;
            attackBuff = new BuffInfo(original, buffed, duration);
            Debug.Log("�U���͂�" + magnification + "�{����܂����B���݂̍U���͂�" + playerStatus.AttackPower + "�ł��B");
        }
    }

    // �ړ����x�Ƀo�t��������
    public void BuffMoveSpeed(float magnification, float duration)
    {
        if (speedBuff != null && speedBuff.isActive)
        {
            speedBuff.timer = 0f;
        }
        else
        {
            AgilityImage.SetActive(true);
            AgilityTimerText.enabled = true;
            float original = playerStatus.Agility;
            float buffed = original * magnification;
            playerStatus.Agility = buffed;
            speedBuff = new BuffInfo(original, buffed, duration);
            Debug.Log("�f������" + magnification + "�{����܂����B���݂̑f������" + playerStatus.Agility + "�ł��B");
        }
    }
}
