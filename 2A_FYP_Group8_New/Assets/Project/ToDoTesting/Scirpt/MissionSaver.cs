using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MissionSaver : Singleton<MissionSaver>
{
    List<missions> takenMissions = new List<missions>();
    // Start is called before the first frame update
    void Start()
    {
        ScenceManager.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeMission(missions mission)
    {
        takenMissions.Add(mission);
    }
    public void DestoryMission(missions mission)
    {
        for (int i = 0; i < takenMissions.Count; i++)
        {
            if (mission == takenMissions[i])
            {
                takenMissions.RemoveAt(i);
                break;
            }
        }
    }

    public List<missions> GetMissionList()
    {
        return takenMissions;
    }
}
