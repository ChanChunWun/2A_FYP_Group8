using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float maxTime = 120;
    float time = 0;
    public TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        time = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        _text.text = "Time: " + time.ToString("000");
        if (time <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            ScenceManager.goScene("StartScene");
        }
    }
}
