using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Input = UnityEngine.Input;

public class PlayerRigidbodyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;

    [Header("Rotation")]
    float rotationVelocity;
    float targetRotation;
    [Range(0.0f, 0.3f)]
    [SerializeField] float RotationSmoothTime = 0.12f;

    [Header("Dash")]
    public bool dashInput = false;
    public bool isDashing = false;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDelay;
    [SerializeField] LayerMask dashCollisionLayers;
    [SerializeField] GameObject dashTarget;

    [Header("Visual Effects")]
    [SerializeField] GameObject dashEffectUp;
    [SerializeField] GameObject dashEffectDown;

    [Header("Juice")]
    [SerializeField] float currentJuice;
    [SerializeField] float maxJuice;

    [Header("References")]
    Rigidbody rb;
    Animator anim;
    [SerializeField] GameObject playerMesh;

    Vector2 moveInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        dashEffectUp.GetComponentInChildren<VisualEffect>().Stop();
        dashEffectDown.GetComponentInChildren<VisualEffect>().Stop();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnDash(InputValue value)
    {
        dashInput = value.isPressed;
    }

    private void Update()
    {
        HandleAnimation();

        if (dashInput && !isDashing)
        {
            StartCoroutine(PlayerDash());
        }
    }


    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        //Disable movement when dashing
        if (isDashing)
            return;

        //Calculates the effect of juice on speed
        float speedMultiplier = (maxJuice - currentJuice) * 0.5f + currentJuice * 1.5f;
        speedMultiplier *= movementSpeed;

        //Creates target velocity from input
        Vector3 horizontalDirInput = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (moveInput != Vector2.zero)
        {
            //Adjusts movement input to camera-facing-based
            targetRotation = Mathf.Atan2(horizontalDirInput.x, horizontalDirInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            //Dampens the player rotation
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        else
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        //Applies the target velocity with move speed modifier
        rb.velocity = targetDirection * speedMultiplier;
    }

    IEnumerator PlayerDash()
    {
        dashInput = false;
        isDashing = true;

        //Grabs the current direction of player
        Vector3 moveDir = rb.velocity.normalized;
        //If player is not moving, dash forward
        if (moveDir == Vector3.zero)
            moveDir = transform.forward;

        //Stops current velocity
        rb.velocity = Vector3.zero;

        //Fires raycast in dash direction
        RaycastHit hit;
        bool dashRayBlocked = false;
        dashRayBlocked = Physics.Raycast(transform.position, moveDir, out hit, dashDistance, dashCollisionLayers);

        //If dash hits an obstacle, place the dash target in front of the obstacle
        if (dashRayBlocked)
        {
            dashTarget.transform.position = hit.point;
        }

        //If dash is unobstructed, place the dash target at the end of the ray
        else
        {
            dashTarget.transform.position = transform.position + moveDir.normalized * dashDistance;
        }

        //Hide player mesh
        playerMesh.SetActive(false);

        dashEffectUp.transform.position = transform.position;
        dashEffectUp.transform.LookAt(dashTarget.transform.position);
        dashEffectUp.GetComponentInChildren<VisualEffect>().Play();

        dashEffectDown.transform.position = dashTarget.transform.position;
        dashEffectDown.transform.LookAt(transform.position);
        dashEffectDown.GetComponentInChildren<VisualEffect>().Play();

        yield return new WaitForSeconds(dashDelay);

        //Move player to dash target and reenable player mesh
        transform.position = dashTarget.transform.position;
        playerMesh.SetActive(true);

        isDashing = false;

        yield return new WaitForSeconds(0.3f);

        dashEffectUp.GetComponentInChildren<VisualEffect>().Stop();
        dashEffectDown.GetComponentInChildren<VisualEffect>().Stop();

        yield return null;
    }

    private void HandleAnimation()
    {
        anim.SetFloat("Locomotion", rb.velocity.magnitude);
    }
}
