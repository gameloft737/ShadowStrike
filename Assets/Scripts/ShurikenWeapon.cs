using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : WeaponObj
{
    
    public GameObject objectToThrow;
    public float throwForce;
    public float throwUpwardForce;

    [SerializeField] WeaponOn weapon;
    [SerializeField] PlayerMovement pm;

    int isThrow;
    private void Update()
    {
        if(Input.GetKeyDown(useKey))
        {
            Use(weaponType.throwable);
        }
        
        
    }
    protected override void Action()
    {
        Debug.Log("trow");
        animator.SetTrigger("Throw");
        weapon.SetRenderer(true, 0f);
        
        if (!pm.isMoving)
        {
            Invoke(nameof(Instance),1.05f);
            weapon.SetRenderer(false, 1f);
        }
        else
        {
            Invoke(nameof(Instance),0.63f);
            weapon.SetRenderer(false, 0.6f);
        }
        //Invoke(nameof(Instance),0f);

    }
    void Instance()
    {
        Vector3 aimDir = (Aiming.hitPos - attackPoint.position).normalized;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));

        projectile.GetComponent<ProjectileAddon>().damage = (int)damage; 

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // add force
        Vector3 forceToAdd = aimDir * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
