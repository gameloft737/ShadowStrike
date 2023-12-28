using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Aiming : MonoBehaviour
{

    [Header ("Ray Settings")]
    [SerializeField] Transform debugTransform;
    [SerializeField] LayerMask aimColliderMask;
    [SerializeField] bool doDebug;
    public static Vector3 hitPos;
    // Update is called once per frame
    void Update()
    {
        RayManager();
    }
    
    protected virtual void RayManager()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            hitPos = raycastHit.point;
            ChangeDebug(raycastHit.point);
        }
        else
        {
            hitPos = ray.GetPoint(99f);
            ChangeDebug(ray.GetPoint(99f));
        }
    }
    void ChangeDebug(Vector3 tran)
    {
        if (doDebug)
        {
            if (debugTransform.gameObject.activeSelf != true)
                debugTransform.gameObject.SetActive(true);
            debugTransform.position = tran;
        }
    }
}
