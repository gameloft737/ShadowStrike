using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMenu : MonoBehaviour
{
    [SerializeField] Animator menuAnim;
    [SerializeField] AnimationCurve scaleAndSlowMoCurve;

    bool isMenuUp = false;

    void Update()
    {
        if(isPlaying(menuAnim, "Hold"))
        {
                
            Time.timeScale = 0.1f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && !isMenuUp)
        {
            
            isMenuUp = true;
            menuAnim.SetTrigger("Open");
            Debug.Log(menuAnim.GetCurrentAnimatorClipInfo(0)[0].clip.ToString());
            
            ThirdPersonCamera.DisableLock();

        }
        else if (Input.GetKeyUp(KeyCode.Q) && isMenuUp)
        {
            
            isMenuUp = false;
            menuAnim.SetTrigger("Close");
            ThirdPersonCamera.EnableLock();
        }
        bool isPlaying(Animator anim, string stateName)
{
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                return true;
            else
                return false;
        }
    }

}
