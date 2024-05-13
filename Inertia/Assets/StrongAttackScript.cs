using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttackScript : MonoBehaviour
{
    [SerializeField] float sizeModifier;
    [SerializeField] float speedModifier;

    Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;

        CinemachineShake.Instance.ShakeCamera(3, sizeModifier);

        Invoke("CleanUp", sizeModifier);
    }

    private void Update()
    {
        transform.localScale += new Vector3(transform.localScale.x + speedModifier, transform.localScale.y + speedModifier, transform.localScale.z + speedModifier) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.InitialiseDamage(10000, 0, false); //Basically Infinite Damage
        }
    }

    void CleanUp()
    {
        Destroy(gameObject);
    }
}
