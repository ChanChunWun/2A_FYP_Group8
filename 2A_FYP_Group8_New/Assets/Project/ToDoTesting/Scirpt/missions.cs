using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;

[CreateAssetMenu(fileName = "New Mission", menuName = "FYP/Mission")]
public class missions : ScriptableObject
{
    public enum missionType
    {
        Annihilate,
        Destroy,
        Boss
    }

    public missionType type;

    public string missionSceneName;

    public string missionID;

    public string missionName;

    public string missionDescribe;

    public string missionTargetText;

    public float missionAward;

    //public bool isDone = false;

    //Annihilate
    public int killNumber;

    //Destroy
    public GameObject[] destroyObject;
    public float timeLimited = 9999;

    // Boss
    public GameObject bossPrefab;

    
}
