using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTimeLine : MonoBehaviour
{
    void OnEnable()
    {
        ScenceManager.goScene("StartScene");
    }

}
