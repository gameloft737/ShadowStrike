using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    
    [SerializeField] Volume v;

    private Vignette vignette;
    [SerializeField] float vignetteSpeed;

    void Start()
    {
        v.profile.TryGet(out vignette);
    }
    // Update is called once per frame
    void Update()
    {
        setVignette();
    }

    void setVignette()
    {
        if (pm.state == PlayerMovement.MovementState.crouching)
        {
            if (vignette.intensity.value < 0.35)
                vignette.intensity.value += vignetteSpeed * Time.deltaTime;
        }
        else
        {
            if (vignette.intensity.value > 0.1)
                vignette.intensity.value -= vignetteSpeed * Time.deltaTime;
        }
    }
}
