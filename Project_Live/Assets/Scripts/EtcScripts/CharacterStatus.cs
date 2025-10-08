using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatus : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] float hp = 100;

    [Header("UŒ‚—Í")]
    [SerializeField] float attackPower = 10f;

    [Header("‘f‘‚³")]
    [SerializeField] float agility = 10f;

    //[Header("”íƒ_ƒ[ƒWŒã‚É”­¶‚·‚é–³“GŠÔ")]
    //[SerializeField] float invincibleDuration = 1f;

    public float Hp { get { return hp; } set { hp = value; } }

    public float AttackPower { get { return attackPower; } set { attackPower = value; } }

    public float Agility { get { return agility; } set { agility = value; } }

    public  float MaxHp { get; private set; }
    
    //public float InvincibleDuration { get { return InvincibleDuration; } set { InvincibleDuration = value; } }

    private void Awake()
    {
        MaxHp = hp;
    }
}
