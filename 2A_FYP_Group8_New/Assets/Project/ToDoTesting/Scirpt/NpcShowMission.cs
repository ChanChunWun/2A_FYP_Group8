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

    List<MissionButton> btnList = new List<MissionButton>();
    // Start is called before the first frame update
    void Start()
    {
        if (npc.mainMissionList.Length > 0)
        {
            SpawnMainMissionButton(npc.GetMainMission());
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
        DestoryMissionButton();
    }
    void SpawnMainMissionButton(missions mission)
    {

        if (mission == null)
            return;

        missionui.ShowMissionStart(mission);
        MissionButton btn_mission = Instantiate(btn_missionPrefab.gameObject, btnSpawnTransform).GetComponent<MissionButton>();
        btn_mission.setName(mission);
        btn_mission.SetMissionUI(missionui);
        btnList.Add(btn_mission);
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
            btnList.Add(btn_mission);
        }
    }

    public void DestoryMissionButton()
    {
        Debug.Log("Doing Check mission");
        if (btnList.Count == 0)
            return;

        for (int i = 0; i < btnList.Count; i++)
        {
            if (btnList[i].myMission.isDone)
            {
                moneyManager.Instance.increaseMoney((int)btnList[i].myMission.missionAward);
                MissionSaver.Instance.DestoryMission(btnList[i].myMission);
                btnList[i].myMission.Reset();
                Destroy(btnList[i].gameObject);
                btnList.RemoveAt(i);
                missionui.ResetMission();
            }
        }

        
    }
}
