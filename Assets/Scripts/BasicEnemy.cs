using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private enum ZombieState
    {
        Walking,
        Idle,
        Ragdoll
    }

    private ZombieState currentState;

    [Header("Stats")]
    public float health;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Camera cam;
    [SerializeField] float attackForce;

    private Rigidbody[] ragdollRigidbodies;
    private Animator animator;
    
    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        DisableRagdoll();
    }
    public void TakeDamage(float damage, Vector3 point)
    {
        health -= damage;

        if (health <= 0)
            DestroyEnemy(attackForce, point);
    }

    public void DestroyEnemy(float forceMagnitude, Vector3 point)
    {
        Vector3 forceDirection = transform.position - cam.transform.position;
        forceDirection.y = 1;
        forceDirection.Normalize();

        Vector3 force = forceMagnitude * forceDirection;
        TriggerRagdoll(force, point);
        //Destroy(gameObject);
    }
    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    { 
        EnableRagdoll();

        Rigidbody hitRigidbody = ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        currentState = ZombieState.Ragdoll;
    } 
      
    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
        //characterController.enabled = true;
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        animator.enabled = false;
        //characterController.enabled = false;
    }
}