using UnityEngine;

public class MatchCollider : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] Vector3 scale;
    [SerializeField] Transform transformer;

    void Update()
    {
        if (pm.state == PlayerMovement.MovementState.crouching || transformer.localScale.y <= 0.5)
        {
            transform.localScale = new Vector3(scale.x, scale.y/pm.crouchYScale, scale.z);
        }
        //ef
        else
        {
            transform.localScale = scale;
        }
        
        
    }
}
