using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class missionUI : MonoBehaviour
{
    public missions showMission;

    public TMP_Text text_Name;
    public TMP_Text text_MissionType;
    public TMP_Text text_Describe;
    public TMP_Text text_TargetScene;
    public TMP_Text text_Target;
    public TMP_Text text_Award;
    public Button btn_Get;
    public Button btn_Finish;
    // Start is called before the first frame update
    void Start()
    {
        ShowMissionStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMissionStart()
    {
        if (showMission == null)
            return;

        text_Name.text = showMission.missionName;
        text_MissionType.text = showMission.type.ToString();
        text_Describe.text = showMission.missionDescribe;
        text_Name.text = showMission.missionName;
        text_Target.text = showMission.missionTargetText;
        text_Award.text = "Award: " + showMission.missionAward.ToString(); 
        
    }

}
