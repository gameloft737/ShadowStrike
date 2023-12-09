using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    [Header("References")]

    [SerializeField] Camera cam;
    [SerializeField]  Transform orientation;
    [SerializeField]  Transform player;
    [SerializeField]  Transform playerObj;
    [SerializeField]  Rigidbody rb;
    public float rotationSpeed;

    [SerializeField]  Transform combatLookAt;

    [SerializeField]  GameObject thirdPersonCam;
    [SerializeField] GameObject combatCam;

    public static CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);


        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if(currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = Vector3.Slerp(orientation.forward,dirToCombatLookAt.normalized,1f);

            playerObj.forward = Vector3.Slerp(playerObj.forward,dirToCombatLookAt.normalized,1f);
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);


        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);


        currentStyle = newStyle;
    }
}
