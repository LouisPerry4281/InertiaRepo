using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackScript : MonoBehaviour
{
    [SerializeField] float sizeModifier;
    [SerializeField] float speedModifier;
    [SerializeField] float timeModifier;

    Vector3 startScale;

    SphereCollider sphereCollider;
    [SerializeField] AnimationCurve animCurve;

    private void Start()
    {
        startScale = transform.localScale;

        sphereCollider = GetComponent<SphereCollider>();

        CinemachineShake.Instance.ShakeCamera(5, sizeModifier);

        StartCoroutine(ColliderGrowth());

        Invoke("CleanUp", timeModifier);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.InitialiseDamage(100, 0, false);
        }
    }

    void CleanUp()
    {
        Destroy(gameObject);
    }

    private IEnumerator ColliderGrowth()
    {
        sphereCollider.radius = 0f;

        float timeStamp = Time.time;
        while (Time.time < timeStamp + timeModifier)
        {
            float t = (Time.time - timeStamp) / timeModifier;
            t = animCurve.Evaluate(t);

            // xPos will move from 0 to 12, non linearly, following the animation curve, and this over 5 seconds
            sphereCollider.radius = Mathf.LerpUnclamped(0f, 12f, t);
            yield return null;
        }
        sphereCollider.radius = 12f;
    }
}
