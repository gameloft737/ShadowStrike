using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform attackPoint;
    [SerializeField] ThirdPersonCamera thirdPersonCamera;

    [Header("General")]
    public float cooldownTime;
    public float damage;
    public int totalUses;

    [SerializeField] protected Animator animator;
    protected KeyCode useKey = KeyCode.Mouse0;
    protected weaponType type;
    public enum weaponType{
        melee,
        throwable
    }

    bool readyToUse;

    void Start()
    {
        thirdPersonCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ThirdPersonCamera>();
        readyToUse = true;
        animator.keepAnimatorStateOnDisable = false;
        
        
    }
    // Update is called once per frame
    protected void Use(weaponType wType)
    {
        
        if (readyToUse && totalUses > 0 && wType == weaponType.throwable)
        {
            readyToUse = false;
            Action();
            totalUses --;
            Invoke(nameof(resetThrow),cooldownTime);
        }
        if (readyToUse && totalUses > 0 && wType == weaponType.melee)
        {
            readyToUse = false;
            Action();
            Invoke(nameof(resetThrow),cooldownTime);
        }
    }
    protected virtual void Action()
    {
        return;
    }

    private void resetThrow()
    {
        thirdPersonCamera.SetPlace();
        readyToUse = true;
    }
}
