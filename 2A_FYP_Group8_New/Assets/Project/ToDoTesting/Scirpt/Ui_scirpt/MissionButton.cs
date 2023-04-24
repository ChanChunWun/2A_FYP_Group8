using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionButton : MonoBehaviour
{
    public string missionName;
    public TMP_Text txt_missionName;
    public missions myMission;
    public missionUI missionui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setName(missions miss)
    {
        missionName = miss.missionName;
        txt_missionName.text = missionName;
        myMission = miss;
    }

    public void SetMissionUI(missionUI ui)
    {
        missionui = ui;
    }

    public void SendThisMissionToSys()
    {
        //missionui.showMission = myMission;
        missionui.ShowMissionStart(myMission);
    }
}
