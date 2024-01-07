using UnityEngine;
using UnityEngine.UI;

public class RotateTowards : MonoBehaviour
{
    public Transform mainCam;
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        // Use Quaternion.LookRotation to make the object face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.position);

        // Update position with the offset relative to the target
        transform.position = target.position + offset;
    }
}