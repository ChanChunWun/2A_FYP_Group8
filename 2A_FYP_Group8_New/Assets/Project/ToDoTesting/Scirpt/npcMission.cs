using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "FYP/NPC")]
public class npcMission : ScriptableObject
{
    public GameObject prefebMissionUI;
    public missions[] mainMissionList;
    public missions[] minorMissionList;
    int mainMissionCount = 0;
    List<string> minorMissionIDs;
    public List<missions> showMinorMissionList;
    public int showMissionNumber = 3;
    // Start is called before the first frame update
    void Start()
    {
        //ResetMinorMission();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainMissionCount + 1 > mainMissionList.Length)
        {
            return;
        }
            

        if (mainMissionList[mainMissionCount].isDone)
        {
            MainMissionPush();
        }
    }

    public missions GetMainMission()
    {
        if (mainMissionCount + 1 > mainMissionList.Length)
        {
            return null;
        }
        else
        {
            return mainMissionList[mainMissionCount];
        }


    }

    void SpawnMission(missions showMis)
    {
        //GameObject missionUI = Instantiate(prefebMissionUI);
        //missionUI.GetComponent<missionUI>().showMission = showMis;
    }

    void ResetMinorMission()
    {
        showMinorMissionList = new List<missions>();
        List<int> missionNum = new List<int>();

        for (int i = 0; i < showMissionNumber; i++)
        {
            if (i == 0)
            {
                missionNum.Add(Random.Range(0, minorMissionList.Length));
                showMinorMissionList.Add(minorMissionList[missionNum[0]]);
            }
            else
            {
                int b = Random.Range(0, minorMissionList.Length);
                for (int a = 0; i < missionNum.Count; a++)
                {
                    while (b == missionNum[a])
                    {
                        b = Random.Range(0, minorMissionList.Length);
                    }
                }
                missionNum.Add(b);
                showMinorMissionList.Add(minorMissionList[missionNum[0]]);
            }
        }
        SetShowMissionID();
    }

    void SetShowMissionID()
    {
        minorMissionIDs = new List<string>();
        for (int i = 0; i < showMinorMissionList.Count; i++)
        {
            minorMissionIDs.Add(showMinorMissionList[i].missionID);
        }
    }

    void MainMissionPush()
    {
        mainMissionCount++;
    }
}
