using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;

    public int destroyTime;
    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        if(!rb.isKinematic)
            Destroy(gameObject, destroyTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
        // check if you hit an enemy
        if(collision.gameObject.tag != "Player")
            // make sure projectile sticks to surface
            rb.isKinematic = true;
            // make sure projectile moves with target
            transform.SetParent(collision.transform);
        if(collision.gameObject.CompareTag("noStick"))
        {
            // destroy projectile
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        
        
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
        // check if you hit an enemy
        if(collision.gameObject.GetComponent<BasicEnemy>() != null)
        {
            
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();
            var collisionPoint = collision.ClosestPoint(transform.position);
            enemy.TakeDamage(damage, collisionPoint);

            // destroy projectile
            Destroy(gameObject);
        }
        if(collision.gameObject.tag != "Player")
            // make sure projectile sticks to surface
            rb.isKinematic = true;
            // make sure projectile moves with target
            transform.SetParent(collision.transform);
        if(collision.gameObject.CompareTag("noStick"))
        {
            // destroy projectile
            Destroy(gameObject);
        }
        
    }
}


