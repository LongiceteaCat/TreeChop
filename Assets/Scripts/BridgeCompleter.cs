using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCompleter : MonoBehaviour
{
    public int woodBridge;
    void Start()
    {
        woodBridge = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(woodBridge>22) Destroy(gameObject);
    }
    
}
