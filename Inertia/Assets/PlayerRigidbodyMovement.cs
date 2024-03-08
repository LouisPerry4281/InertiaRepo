using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerRigidbodyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float movementSpeed;

    [Header("Rotation")]
    float rotationVelocity;
    float targetRotation;
    [Range(0.0f, 0.3f)]
    [SerializeField] float RotationSmoothTime = 0.12f;

    [Header("Juice")]
    [SerializeField] float currentJuice;
    [SerializeField] float maxJuice;

    [Header("References")]
    Rigidbody rb;

    Vector2 moveInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        //Applies the target velocity with move speed modifier
        rb.velocity = targetDirection * speedMultiplier;
    }
}
