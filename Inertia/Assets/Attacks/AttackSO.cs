using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
