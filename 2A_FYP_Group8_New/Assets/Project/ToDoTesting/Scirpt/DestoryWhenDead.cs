using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryWhenDead : MonoBehaviour
{
    LifeSystem lf;
    // Start is called before the first frame update
    void Start()
    {
        lf = GetComponent<LifeSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lf.dead)
            Destroy(gameObject);
    }
}
