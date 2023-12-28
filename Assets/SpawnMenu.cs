using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMenu : MonoBehaviour
{
    [SerializeField] Image menuImage;
    [SerializeField] AnimationCurve scaleAndSlowMoCurve;

    bool isMenuUp = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isMenuUp)
        {
            StartCoroutine(PopUpMenuCoroutine(new Vector3(1f, 1f, 1f), 1.5f, scaleAndSlowMoCurve));
        }
        else if (Input.GetKeyUp(KeyCode.Q) && isMenuUp)
        {
            StartCoroutine(PopUpMenuCoroutine(new Vector3(0f, 0f, 0f), 1.5f, scaleAndSlowMoCurve));
        }
    }

    IEnumerator PopUpMenuCoroutine(Vector3 targetScale, float duration, AnimationCurve curve)
    {
        isMenuUp = targetScale != Vector3.zero;
        float initialTimeScale = Time.timeScale;
        Vector3 initialScale = menuImage.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float curveValue = curve.Evaluate(progress);

            menuImage.transform.localScale = Vector3.Lerp(initialScale, targetScale, curveValue);
            Time.timeScale = Mathf.Lerp(initialTimeScale, curveValue, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set
        menuImage.transform.localScale = targetScale;

        // Reset time scale outside the loop
        Time.timeScale = isMenuUp ? Mathf.Lerp(initialTimeScale, 0.1f, 0.1f) : 1f;
    }
}
