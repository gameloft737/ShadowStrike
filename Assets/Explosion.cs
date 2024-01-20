using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] GameObject particles;
    public void Explode(float radius, float force, float upliftModifier)
    {
        if (particles != null)
        {
            var p = Instantiate(particles, transform.position, transform.rotation);
            p.GetComponent<LimitedLife>().Die(1.5f);
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb.tag != "Player")
            {
                rb.AddExplosionForce(rb.mass*force, transform.position, radius, upliftModifier);
            }
        }
    }
}
