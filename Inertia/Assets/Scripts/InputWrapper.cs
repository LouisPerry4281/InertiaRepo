using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWrapper : MonoBehaviour
{
	public Vector2 move;
	public Vector2 look;
	public bool dash;

	public void OnMove(InputValue value)
    {
		move = value.Get<Vector2>();
    }

	public void OnLook(InputValue value)
    {
		look = value.Get<Vector2>();
    }

	public void OnDash(InputValue value)
    {
		dash = value.isPressed;
    }
}
