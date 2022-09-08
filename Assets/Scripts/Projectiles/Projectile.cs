using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int heal;
    //effect
    public float moveSpeed;

    private Character target;

    public void Initialized(Character targetChar)
    {
        target = targetChar;
    }
    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + new Vector3(0f,0.5f,0f) , moveSpeed * Time.deltaTime);
        }
    }
    void ImpactTarget()
    {
        if (damage > 0)
            target.TakeDamage(damage);

        if (heal > 0)
        {
            target.Heal(heal);
        }

        //effect
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            ImpactTarget();
            Destroy(gameObject);
        }
    }
}
