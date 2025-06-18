using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static EventHandler<ComboAttackEventArgs> ComboAttack;
    public static EventHandler ComboReset;
    public class ComboAttackEventArgs : EventArgs
    {
        public int damage;
    }

    [Header("Core")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerModel;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    private Vector3 moveDirection;


    [Header("Attack")]
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private BoxCollider axeCollider;
    private int comboStep = 0;
    private bool comboQueued = false;



    private bool isAttacking = false;
    private bool canAttack = true;
    private bool isIdle = true;

    private void Start()
    {
        axeCollider.enabled = false;
    }
    private void OnEnable()
    {
        attackInput.action.started += Attack;
        AnimationController.OnAttackAnimationEnd += EndAttack;
        ComboAttack += Player_ComboAttack;
        ComboReset += Player_ComboReset;
    }
    private void OnDisable()
    {
        attackInput.action.started -= Attack;
        AnimationController.OnAttackAnimationEnd -= EndAttack;
        ComboAttack -= Player_ComboAttack;
        ComboReset -= Player_ComboReset;
    }
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (isAttacking) return;

        animator.SetBool("isRunning", true);
        playerModel.transform.localPosition = Vector3.zero;
        playerModel.transform.localRotation = Quaternion.identity;

        var inputDirection= moveInput.action.ReadValue<Vector2>();
        moveDirection= new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

        if (moveDirection.magnitude>=0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Vector3 finalMove = moveDir.normalized * movementSpeed;

            characterController.Move(finalMove * Time.deltaTime);

            isIdle = false;
        }
        else
        {
            if (animator.GetBool("isRunning"))
            {
                animator.SetBool("isRunning", false);
                isIdle = true;
            }
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!isAttacking && canAttack)
        {
            //first attack
            axeCollider.enabled = true;
            comboStep = 1;
            isAttacking = true;
            canAttack = false;
            animator.SetTrigger("Attack1");
        }
        else if (isAttacking && !comboQueued)
        {
            animator.SetBool("isCombo", true);
            comboQueued = true;
        }
    }

    public void EndAttack()
    {
        if (comboQueued && comboStep < 3)
        {
            ComboAttack?.Invoke(this, new ComboAttackEventArgs { damage = attackDamage * comboStep });
        }
        else
        {
            ComboReset?.Invoke(this,null);
        }
    }
    private void Player_ComboAttack(object sender, ComboAttackEventArgs args)
    {
        axeCollider.enabled = true;
        Debug.Log("Combo" + comboStep);
        comboStep++;
        animator.SetTrigger("Attack" + comboStep);
        comboQueued = false;
    }
    private void Player_ComboReset(object sender, EventArgs args)
    {
        axeCollider.enabled = false;
        animator.SetBool("isCombo", false);
        comboStep = 0;
        isAttacking = false;
        canAttack = true;
        comboQueued = false;
    }
}
