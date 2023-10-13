using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchCollider : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] Vector3 scale;

    void Update()
    {
        if (pm.state == PlayerMovement.MovementState.crouching)
        {
            transform.localScale = new Vector3(scale.x, scale.y/pm.crouchYScale, scale.z);
        }
        else
        {
            transform.localScale = scale;
        }
        
        
    }
}
