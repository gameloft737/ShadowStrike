using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;

    public int destroyTime;
    private Rigidbody rb;
    public bool doExplosions;
    public bool isDetonation;
    public bool radiusExplosion;
    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        if(!rb.isKinematic && !isDetonation)
            Destroy(gameObject, destroyTime);
        if (isDetonation && Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("BOOM");
            Detonate();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if(isDetonation)
        {
            return;
        }
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;
        if(collision.gameObject.tag == "Player")
            Destroy(gameObject);
        // check if you hit an enemy
        if(collision.gameObject.tag != "Player")
        {
            if(doExplosions & !isDetonation)
            {
                GetComponent<Explosion>().Explode(5f,500,5f);
                Destroy(gameObject);
            }
            else
            {
                // make sure projectile sticks to surface
                rb.isKinematic = true;
                // make sure projectile moves with target
                transform.SetParent(collision.transform);
            }
            
        }
        if(collision.gameObject.CompareTag("noStick"))
        {
            // destroy projectile
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(!isDetonation)
        {
            return;
        }
        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;
        if(collision.gameObject.tag == "Player")
            return;
        
        if(collision.gameObject.tag != "Player")
            // make sure projectile sticks to surface
            rb.isKinematic = true;
            // make sure projectile moves with target
            transform.SetParent(collision.transform, true);
        if(collision.gameObject.CompareTag("noStick"))
        {
            // destroy projectile
            Destroy(gameObject);
        }
    }
    public void Detonate()
    {
        GetComponent<Explosion>().Explode(7f,700,10f);
        Destroy(gameObject);
    }
}


