using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(!HasResources())
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Anvil");
        }
    }
    bool HasResources()
    {
        return true;
    }
}
