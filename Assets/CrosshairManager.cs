using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        handleSprite();
    }
    void handleSprite()
    {

        image.enabled = ThirdPersonCamera.currentStyle == ThirdPersonCamera.CameraStyle.Combat;


    }
}
