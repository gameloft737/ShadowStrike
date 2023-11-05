using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOn : MonoBehaviour
{
    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Toggle the Object's visibility each second.
    public void SetRenderer(bool isOn, float time)
    {
        if (isOn)
            Invoke(nameof(SetOn),time);
        else
            Invoke(nameof(SetOff),time);
        
    }
    void SetOn()
    {
        rend.enabled = true;
    }
    void SetOff()
    {
        rend.enabled = false;
    }
}
