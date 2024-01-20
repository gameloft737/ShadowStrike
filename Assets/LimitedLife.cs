using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedLife : MonoBehaviour
{
    public void Die(float s)
    {
        StartCoroutine(SetBack(s));
    }
    private IEnumerator SetBack(float f)
    {
        yield return new WaitForSeconds(f);
        Destroy(gameObject);

    }
}
