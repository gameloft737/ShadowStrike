using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private KeyCode aimKey = KeyCode.Mouse1;

    [SerializeField] float aimFov;

    [SerializeField] float aimSpeed;

    public static bool isAiming;
    CinemachineFreeLook freeLook;
    // Start is called before the first frame update
    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
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
}
