using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryInSc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScenceManager.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
