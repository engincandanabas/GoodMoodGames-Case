using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private Transform mainCamera;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference input;
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    private Vector3 moveDirection;



    private bool canAttack = true;
    private bool isIdle = true;

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var inputDirection=input.action.ReadValue<Vector2>();
        moveDirection= new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

        if (moveDirection.magnitude>=0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            Vector3 finalMove = moveDir.normalized * movementSpeed;

            characterController.Move(finalMove * Time.deltaTime);
        }
    }


}
