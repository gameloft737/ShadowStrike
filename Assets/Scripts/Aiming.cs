using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private KeyCode aimKey = KeyCode.Mouse1;

    [Header ("Aim Settings")]
    [SerializeField] float aimFov;
    [SerializeField] float aimSpeed;

    [Header ("Ray Settings")]
    [SerializeField] Transform debugTransform;
    [SerializeField] LayerMask aimColliderMask;
    public static bool isAiming;
    public static Vector3 hitPos;
    [SerializeField] CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }
    // Update is called once per frame
    void Update()
    {
        AimManager();
        RayManager();
    }
    protected virtual void AimManager()
    {
        if(Input.GetKey(aimKey))
        {
            isAiming = true;
            if (freeLook.m_Lens.FieldOfView > aimFov)
                freeLook.m_Lens.FieldOfView -= aimSpeed * Time.deltaTime;
        }
        else
        {
            isAiming = false;
            if (freeLook.m_Lens.FieldOfView < 50)
                freeLook.m_Lens.FieldOfView += aimSpeed * Time.deltaTime;
            else
                freeLook.m_Lens.FieldOfView = 50f;
        }
    }

    protected virtual void RayManager()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            hitPos = raycastHit.point;
        }
        else
        {
            hitPos = ray.GetPoint(99f);
        }
    }
}
