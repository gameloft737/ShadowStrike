using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    UnityEngine.UI.Image image;
    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }
    // Update is called once per frame
    void Update()
    {
        HandleSprite();
    }
    void HandleSprite()
    {

        image.enabled = ThirdPersonCamera.currentStyle == ThirdPersonCamera.CameraStyle.Combat;


    }
}
