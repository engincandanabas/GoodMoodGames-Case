using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System;

public class CameraController : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private CameraType cameraType;

    [Header("Core")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform player;
    [SerializeField] private InputActionReference input;


    [Header("LockOn Settings")]
    [SerializeField] private float smoothTime = 0.2f;
    private float currentYawVelocity;


    private CinemachineOrbitalFollow orbitalFollow;


    void Start()
    {
        orbitalFollow = cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
    }
    private void OnEnable()
    {
        input.action.started += ChangeCameraType;
    }

    

    private void OnDisable()
    {
        
    }
    void Update()
    {
        LockOnCamera();
    }
    private void ChangeCameraType(InputAction.CallbackContext context)
    {
        cameraType = (cameraType == CameraType.FreeLook) ? CameraType.LockOn : CameraType.FreeLook;
    }
    private void LockOnCamera()
    {
        if (cameraType == CameraType.LockOn)
        {
            Transform nearestEnemy = FindClosestEnemy(player.transform, "Enemy");
            if (nearestEnemy == null || player == null || orbitalFollow == null) return;

            Vector3 dir = nearestEnemy.position - player.position;
            dir.y = 0; // only horizontal

            if (dir.sqrMagnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

                // smooth damp
                float currentAngle = orbitalFollow.HorizontalAxis.Value;
                float smoothedAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentYawVelocity, smoothTime);

                orbitalFollow.HorizontalAxis.Value = smoothedAngle;
            }
        }
    }
    private Transform FindClosestEnemy(Transform player, string enemyTag, float maxDistance = Mathf.Infinity)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        Transform closest = null;
        float closestDistanceSqr = maxDistance * maxDistance;

        Vector3 playerPos = player.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 dirToEnemy = enemy.transform.position - playerPos;
            float dSqrToEnemy = dirToEnemy.sqrMagnitude;

            if (dSqrToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToEnemy;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
public enum CameraType
{
    FreeLook,
    LockOn
}