using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputWrapper _input;
    private HitboxManager _hitboxManager;

    public GameObject CinemachineCameraTarget;
    private CharacterController _controller;
    private Animator _playerAnim;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    public float globalSpeed;
    public float speedChangeRate;

    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    public float _maxJuice;
    public float _currentJuice;

    public bool isDashing = false;
    public float dashModifier;
    public float dashTime;
    private Vector3 preDashVelocity;
    private Vector3 dashVelocity;

    private const float _threshold = 0.01f;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        _input = GetComponent<InputWrapper>();
        _controller = GetComponent<CharacterController>();
        _playerAnim = GetComponentInChildren<Animator>();
        _hitboxManager = GetComponentInChildren<HitboxManager>();

    }

    private void Update()
    {
        MovePlayer();
        Dash();
    }

    private void MovePlayer()
    {
        if (isDashing)
            return;

        float speedMultiplier = (_maxJuice - _currentJuice) * 0.5f + _currentJuice * 1.5f;
        
        float targetSpeed = globalSpeed * speedMultiplier;

        if (_input.move == Vector2.zero)
            targetSpeed = 0;

        float currentSpeed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        if (currentSpeed < targetSpeed - speedOffset || currentSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }

        else
            _speed = targetSpeed;

        Vector3 horizontalDirInput = new Vector3(_input.move.x, 0, _input.move.y).normalized;

        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(horizontalDirInput.x, horizontalDirInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime)); //When adding gravity/Jump Don't forget to +verticalVelocity

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        HandleAnimation();
    }

    private void HandleAnimation()
    {
        _playerAnim.SetFloat("Locomotion", _speed);
    }

    private void Dash()
    {
        if (!_hitboxManager.insideEnemy && !isDashing)
        {
            //Re-enables player and enemy collisions
            Physics.IgnoreLayerCollision(7, 6, false);
        }

        if (!_input.dash)
            return;

        StartCoroutine(DashController());

        if (isDashing)
            _controller.Move(dashVelocity);
    }

    private IEnumerator DashController()
    {
        isDashing = true;

        //Disables collision between player and enemy layer
        Physics.IgnoreLayerCollision(7, 6, true);

        //Determines direction player is moving when starting dash and adjusting for modifier variable and deltaTime (as well as normalising for direction)
        preDashVelocity = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).normalized;
        dashVelocity = preDashVelocity * dashModifier * Time.deltaTime;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        _input.dash = false;

        yield return null;
    }

    private void LateUpdate()
    {
        //CameraRotation();
    }

    public void JuiceChange(float juiceToAdd)
    {
        _currentJuice += juiceToAdd;
        _currentJuice = Mathf.Clamp(_currentJuice, 0, _maxJuice);
    }

    #region CameraStuff
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void CameraRotation()
    {
        if (isDashing)
            return;

        if (_input.look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _input.look.x;
            _cinemachineTargetPitch += _input.look.y;
        }

        //Clamp rotations so values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        //Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }
    #endregion
}
