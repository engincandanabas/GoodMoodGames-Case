using System;
using UnityEngine;
using static Player;

public class PlayerTrigger : MonoBehaviour
{
    private int attackDamage = 10;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
        }
    }
    private void OnEnable()
    {
        ComboAttack += Player_ComboAttack;
        ComboReset += Player_ComboReset;
    }
    private void OnDisable()
    {
        ComboAttack -= Player_ComboAttack;
        ComboReset -= Player_ComboReset;
    }
    private void Player_ComboAttack(object sender, ComboAttackEventArgs args)
    {
        attackDamage = args.damage;
    }
    private void Player_ComboReset(object sender, EventArgs args)
    {
        attackDamage = 10;
    }
}

