using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcShowMission : MonoBehaviour
{
    [SerializeField]
    missionUI missionui;
    [SerializeField]
    MissionButton btn_missionPrefab;
    [SerializeField]
    npcMission npc;
    [SerializeField]
    Transform btnSpawnTransform;
    // Start is called before the first frame update
    void Start()
    {
        if (npc.mainMissionList.Length > 0)
        {
            SpawnMissionButton(npc.mainMissionList);
            //missionui.showMission = npc.mainMissionList[0];
            //missionui.ShowMissionStart();
        }
        if (npc.minorMissionList.Length > 0)
        {
            SpawnMissionButton(npc.minorMissionList);
            //Debug.Log("Mission Name: " + npc.minorMissionList[0].missionName);
            //missionui.showMission = npc.minorMissionList[0];
            //missionui.ShowMissionStart();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnMissionButton(missions[] missionList)
    {
        if (missionList.Length <= 0)
            return;

        missionui.ShowMissionStart(missionList[0]);
        for (int i = 0; i < missionList.Length; i++)
        {
            MissionButton btn_mission = Instantiate(btn_missionPrefab.gameObject, btnSpawnTransform).GetComponent<MissionButton>();
            btn_mission.setName(missionList[i]);
            btn_mission.SetMissionUI(missionui);
            
        }
    }
}
