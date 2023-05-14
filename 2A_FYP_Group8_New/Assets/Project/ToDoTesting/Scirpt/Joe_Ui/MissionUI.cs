using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public missions show;

    public TMP_Text missionNameText;
    public TMP_Text missionTargetNameText;
    public TMP_Text missionNowNumberText;
    public TMP_Text missionMaxNumberText;

    private void Update()
    {
        SetUi();
    }

    public void SetMission(missions mission)
    {
        show = mission;
    }

    public void SetUi()
    {
        missionNameText.text = show.missionName;
        missionTargetNameText.text = show.missionTargetName;
        missionNowNumberText.text = show.killedNumber.ToString();
        missionMaxNumberText.text = show.killNumber.ToString();

    }
}
