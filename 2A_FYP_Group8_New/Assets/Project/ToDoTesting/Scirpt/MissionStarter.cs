using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionStarter : MonoBehaviour
{
    // Start is called before the first frame update
    Transform BossSpawnTransform;
    List<Transform> DestoryObjectsSpawnTransform = new List<Transform>();

    void Start()
    {
        FindSpawnTransforms();
        StartMission();
    }

    void FindSpawnTransforms()
    {
        BossSpawnTransform = GameObject.Find("BossSpawnTransform").transform;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("DesSpawnPos");
        foreach (GameObject spawnPoint in spawnPoints)
        {
            DestoryObjectsSpawnTransform.Add(spawnPoint.transform);
        }
    }

    void StartMission()
    {
        List<missions> startMissions = MissionSaver.Instance?.GetMissionList();
        if (startMissions.Count == 0)
            return;

        foreach (missions mission in startMissions)
        {
            if (mission.missionSceneName == SceneManager.GetActiveScene().name)
            {
                if (mission.type == missions.missionType.Annihilate)
                {

                }
                else if (mission.type == missions.missionType.Destroy)
                {
                    for (int i = 0; i < mission.destroyObject.Length; i++)
                    {
                        Instantiate(mission.destroyObject[i], DestoryObjectsSpawnTransform[i]);
                    }
                }
                else if (mission.type == missions.missionType.Boss)
                {
                    Instantiate(mission.bossPrefab, BossSpawnTransform);
                }
            }
        }
    }
}
