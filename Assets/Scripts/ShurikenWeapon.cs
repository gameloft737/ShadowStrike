using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : WeaponObj
{
    public GameObject objectToThrow;
    public float throwForce;
    public float throwUpwardForce;

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

        //Invoke(nameof(Instance),0.55f);
        Invoke(nameof(Instance),0f);

    }
    void Instance()
    {
        Quaternion q = Quaternion.FromToRotation(Vector3.up, transform.forward);
        
        objectToThrow.transform.rotation = q * attackPoint.transform.rotation;

         GameObject projectile = Instantiate(objectToThrow, attackPoint.position, q);

         projectile.GetComponent<ProjectileAddon>().damage = (int)damage; 

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
