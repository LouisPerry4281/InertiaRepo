using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputWrapper : MonoBehaviour
{
	public Vector2 move;
	public Vector2 look;
	public bool dash;
	public bool attack;

	public void OnMove(InputValue value) //WASD
    {
		move = value.Get<Vector2>();
    }

	public void OnLook(InputValue value) //MouseLook
    {
		look = value.Get<Vector2>();
    }

	public void OnDash(InputValue value) //LeftShift
    {
		dash = value.isPressed;
    }
	
	public void OnAttack(InputValue value) //LeftMouse
    {
		attack = value.isPressed;
    }
}
