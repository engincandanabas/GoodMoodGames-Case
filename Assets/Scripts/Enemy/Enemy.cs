using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Core")]
    [SerializeField] private int health = 100;
    [SerializeField] private float damageCooldown = 0.3f;
    [SerializeField] private Image healthImage;

    private Animator animator;
    private BoxCollider boxCollider;
    private bool isTakingDamage = false;

    private int maxHealth;

    private void Start()
    {
        maxHealth = health;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void TakeDamage(int damage)
    {
        if (isTakingDamage) return;


        isTakingDamage = true;
        health -= damage;

        DOTween.Kill(healthImage);
        healthImage.DOFillAmount((float)health / (float)maxHealth, 0.2f);

        if (health <= 0)
        {
            animator.SetTrigger("isDeath");
            boxCollider.enabled = false;
            EnemySpawner.Instance.SpawnEnemy();
        }
        else
        {
            animator.SetTrigger("takeDamage");
            Invoke(nameof(ResetDamageState), damageCooldown);
        }
    }

    private void ResetDamageState()
    {
        isTakingDamage = false;
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
