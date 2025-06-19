using System;
using System.Collections;
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


            PopUpSpawner.Instance.GetPopUp(other.transform.position+
                new Vector3(UnityEngine.Random.Range(-1,1),UnityEngine.Random.Range(0.5f,1.1f),0),
                attackDamage);

            StartCoroutine(CameraController_ShakeCamera());
            AudioManager.Instance.PlaySfx("Damage");
        }
        this.GetComponent<BoxCollider>().enabled = false;
        
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
    private IEnumerator CameraController_ShakeCamera()
    {
        yield return new WaitForSeconds(0.3f);
        CameraController.Instance.ShakeCamera();
    }
}

