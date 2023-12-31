using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObj : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform attackPoint;

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
        readyToUse = true;
    }
}
