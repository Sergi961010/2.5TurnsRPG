using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleVisuals : MonoBehaviour
{
    const string LEVEL_ABB = "Lvl: ";
    const string IS_ATTACKING_PARAM = "IsAttacking";
    const string IS_HIT_PARAM = "IsHit";
    const string IS_DEAD_PARAM = "IsDead";

    Animator animator;
    int currentHealth;
    int maxHealth;
    int level;

    [SerializeField] Slider healthBar;
    [SerializeField] TextMeshProUGUI levelText;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetStartingValues(int currentHealth, int maxHealth, int level)
    {
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.level = level;
        levelText.text = string.Concat(LEVEL_ABB, this.level.ToString());

        UpdateHealthBar();
    }

    public void UpdateHealthBar(int currentHealth)
    {
        this.currentHealth = currentHealth;

        if (currentHealth <= 0)
        {
            PlayDeadAnimation();
            Destroy(gameObject, 1f);
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(IS_ATTACKING_PARAM);
    }

    public void PlayHitAnimation()
    {
        animator.SetTrigger(IS_HIT_PARAM);
    }

    public void PlayDeadAnimation()
    {
        animator.SetTrigger(IS_DEAD_PARAM);
    }
}
