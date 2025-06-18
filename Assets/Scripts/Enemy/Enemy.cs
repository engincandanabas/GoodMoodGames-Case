using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Core")]
    [SerializeField] private int health = 100;
    [SerializeField] private float damageCooldown = 0.3f;

    private Animator animator;
    private BoxCollider boxCollider;
    private bool isTakingDamage = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void TakeDamage(int damage)
    {
        if (isTakingDamage) return;

        isTakingDamage = true;
        health -= damage;

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
