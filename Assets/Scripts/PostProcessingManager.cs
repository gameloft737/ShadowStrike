using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    
    [SerializeField] Volume v;

    private Vignette vignette;
    private ChromaticAberration aberration;
    [SerializeField] float vignetteSpeed;

    void Start()
    {
        v.profile.TryGet(out vignette);
        v.profile.TryGet(out aberration);
    }
    // Update is called once per frame
    void Update()
    {
        SetVignette();
        SetAbberation();
    }

    void SetVignette()
    {
        if (pm.state == PlayerMovement.MovementState.crouching)
        {
            if (vignette.intensity.value < 0.35)
                vignette.intensity.value += vignetteSpeed * Time.deltaTime;
        }
        else if (ThirdPersonCamera.currentStyle == ThirdPersonCamera.CameraStyle.Combat)
        {
            if (vignette.intensity.value < 0.25)
                vignette.intensity.value += vignetteSpeed * Time.deltaTime;
            else if (vignette.intensity.value > 0.25)
                vignette.intensity.value -= vignetteSpeed * Time.deltaTime;
        }
        else
        {
            if (vignette.intensity.value > 0.1)
                vignette.intensity.value -= vignetteSpeed * Time.deltaTime;
        }
        
        
    }

    void SetAbberation()
    {
        if (pm.state == PlayerMovement.MovementState.air)
        {
            if (aberration.intensity.value < 0.8)
                aberration.intensity.value += vignetteSpeed * 8 * Time.deltaTime;
        }
        else
        {
            if (aberration.intensity.value > 0)
                aberration.intensity.value -= vignetteSpeed * 4 * Time.deltaTime;
        }
    }
}
