using UnityEngine;
using UnityEngine.UI;

public class RotateTowards :MonoBehaviour
{
    public Transform mainCam;
    public Transform target;
    public Transform worldSpaceCanvas;
    public Vector3 offset;


    private void Start ()
    {
        mainCam = Camera.main.transform;
        transform.SetParent(worldSpaceCanvas);

    }
    void Update ()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position); // look at camera

        transform.position = target.position + offset;

    }
}