using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarrokStats : MonoBehaviour
{
    public Animator animator;
    public Slider healthBar;
    public GameObject Warrok;
    public Animator anim;

    public float maxHealth = 100.0f;
    public float baseAttack = 10.0f;
    public float currentHealth { get; set; }
    public float attackPower { get; set; } 
    public float defense = 5.0f;
    public float speed = 10.0f;

    public bool isDead;

    // Initialize the character's stats
    void Start()
    {
        currentHealth = maxHealth;
        attackPower = baseAttack;
    }
    void Update()
    {
        if (Warrok.GetComponent<WarrokStateManager>().isRaging == true)
        {
            IncreaseAttackPower(15.3f);
        }
    }

    // Function to take damage
    public void TakeDamage(float damage)
    {
        float actualDamage = damage - defense;
        if (actualDamage < 0)
        {
            actualDamage = 0;
        }
        currentHealth -= actualDamage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    // Function to heal
    public void HealSpell(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Function to increase attack power
    public void IncreaseAttackPower(float amount)
    {
        attackPower += amount;
    }

    // Function to increase defense
    public void IncreaseDefense(float amount)
    {
        defense += amount;
    }

    // Function to increase speed
    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

}
