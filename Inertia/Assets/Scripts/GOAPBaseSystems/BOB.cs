using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BOB : GAgent
{
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);
    }
}
