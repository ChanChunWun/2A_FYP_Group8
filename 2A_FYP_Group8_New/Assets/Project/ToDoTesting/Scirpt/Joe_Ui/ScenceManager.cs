using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceManager : MonoBehaviour
{
    // Start is called before the first frame update\

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public static void goScene(string scName)
    {
        SceneManager.LoadScene(scName);
    }


}
