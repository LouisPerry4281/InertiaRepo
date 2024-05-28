using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    Weapon weapon;

    private void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    public void TriggerOn()
    {
        weapon.EnableTrigger();
    }

    public void TriggerOff()
    {
        weapon?.DisableTrigger();
    }
}
