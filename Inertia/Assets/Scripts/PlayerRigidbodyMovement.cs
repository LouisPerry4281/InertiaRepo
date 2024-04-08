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
    Vector3 targetDirection;

    [Header("Rotation")]
    float rotationVelocity;
    float targetRotation;
    [Range(0.0f, 0.3f)]
    [SerializeField] float RotationSmoothTime = 0.12f;

    [Header("Dash")]
    public bool dashInput = false;
    public bool isDashing = false;
    bool canDash = true;
    [SerializeField] float dashDistance;
    [SerializeField] float dashDelay;
    [SerializeField] float dashCooldown;
    [SerializeField] LayerMask dashCollisionLayers;
    [SerializeField] GameObject dashTarget;
    [SerializeField] float wallDashBuffer;

    [Header("Visual Effects")]
    [SerializeField] GameObject dashEffectUp;
    [SerializeField] GameObject dashEffectDown;
    VisualEffect actionLines;
    [SerializeField] float actionLineMaxSpawnRate;

    [Header("Juice")]
    public float currentJuice;
    [SerializeField] float maxJuice;

    [Header("References")]
    Rigidbody rb;
    Animator anim;
    PlayerRigidbodyCombat combatScript;
    [SerializeField] GameObject playerMesh;

    Vector2 moveInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        combatScript = GetComponent<PlayerRigidbodyCombat>();

        dashEffectUp.GetComponentInChildren<VisualEffect>().Stop();
        dashEffectDown.GetComponentInChildren<VisualEffect>().Stop();

        actionLines = GameObject.Find("ActionLines").GetComponent<VisualEffect>();
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
        HandleActionLines();

        if (dashInput && !isDashing && canDash)
        {
            StartCoroutine(PlayerDash());
        }

        else
            dashInput = false;
    }

    public void JuiceChange(float juiceAmount)
    {
        currentJuice += juiceAmount;
        currentJuice = Mathf.Clamp(currentJuice, 0, maxJuice);

        FindAnyObjectByType<JuiceBarScript>().SetJuice(currentJuice);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
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

        targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        //Disable movement when dashing
        if (isDashing /*|| combatScript.isAttacking*/)
            return;

        //Applies the target velocity with move speed modifier
        rb.velocity = targetDirection * speedMultiplier;
    }

    IEnumerator PlayerDash()
    {
        dashInput = false;
        isDashing = true;
        canDash = false;

        //Grabs the current direction of player
        Vector3 moveDir = targetDirection;
        //If player is not moving, dash forward
        if (moveDir == Vector3.zero)
            moveDir = transform.forward;

        //Stops current velocity
        rb.velocity = Vector3.zero;

        yield return new WaitForFixedUpdate();

        //Fires raycast in dash direction
        RaycastHit hit;
        bool dashRayBlocked = false;
        dashRayBlocked = Physics.Raycast(transform.position, moveDir, out hit, dashDistance, dashCollisionLayers);

        //If dash hits an obstacle, place the dash target in front of the obstacle
        if (dashRayBlocked)
        {
            Vector3 dashTargetPosition = ((hit.point - transform.position) / wallDashBuffer) + transform.position;

            dashTarget.transform.position = dashTargetPosition;
        }

        //If dash is unobstructed, place the dash target at the end of the ray
        else
        {
            dashTarget.transform.position = transform.position + moveDir.normalized * dashDistance;
        }

        //Hide player mesh
        playerMesh.SetActive(false);

        float verticalOffset = 1;

        dashEffectUp.transform.position = new Vector3(transform.position.x, transform.position.y + verticalOffset, transform.position.z);
        dashEffectUp.transform.LookAt(dashTarget.transform.position);
        dashEffectUp.GetComponentInChildren<VisualEffect>().Play();

        dashEffectDown.transform.position = new Vector3(dashTarget.transform.position.x, dashTarget.transform.position.y + verticalOffset, dashTarget.transform.position.z);
        dashEffectDown.transform.LookAt(transform.position);
        dashEffectDown.GetComponentInChildren<VisualEffect>().Play();

        //Slowly move the invisible player to the dash location
        float elapsedTime = 0;
        while (elapsedTime < dashDelay)
        {
            transform.position = Vector3.Lerp(transform.position, dashTarget.transform.position, (elapsedTime/dashDelay));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //Hard move player to dash target and reenable player mesh
        transform.position = dashTarget.transform.position;
        playerMesh.SetActive(true);

        isDashing = false;

        yield return new WaitForSeconds(0.3f);

        dashEffectUp.GetComponentInChildren<VisualEffect>().Stop();
        dashEffectDown.GetComponentInChildren<VisualEffect>().Stop();

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

        yield return null;
    }

    private void HandleAnimation()
    {
        if (combatScript.isAttacking)
        {
            return;
        }

        anim.SetFloat("Locomotion", rb.velocity.magnitude);
    }

    private void HandleActionLines()
    {
        //Take the current speed and normalise it to a 0-1 scale
        float currentSpeed = rb.velocity.magnitude;
        float normalisedSpeed = currentSpeed / 12; //12 being the max speed the player can reach rounded up

        //Only applies action lines when player is 70% or more of their max speed
        if (normalisedSpeed > 0.7f)
            actionLines.SetFloat("Spawn", normalisedSpeed * actionLineMaxSpawnRate);
        else
            actionLines.SetFloat("Spawn", 0);
    }
}
