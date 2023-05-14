using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionUIManager : Singleton<MissionUIManager>
{
    public MissionUI missionUIPrefab;
    public Transform MissionUISpawnTranform;
    List<MissionUI> missionUis = new List<MissionUI>();
    // Start is called before the first frame update
    void Start()
    {
        if (MissionSaver.Instance.GetMissionList().Count > 0)
        {
            startSpawnMissionUI();
        }
    }

    private void Update()
    {

    }

    public void startSpawnMissionUI()
    {
        missionUis = new List<MissionUI>();
        List<missions> showList = MissionSaver.Instance.GetMissionList();
        if (showList.Count == 0)
            return;

        foreach (missions mission in showList)
        {
            if (mission.missionSceneName == SceneManager.GetActiveScene().name)
            {
                MissionUI spawnUI = Instantiate(missionUIPrefab.gameObject, MissionUISpawnTranform).GetComponent<MissionUI>();
                spawnUI.SetMission(mission);
                spawnUI.missionNameText.text = mission.missionName;
                spawnUI.missionTargetNameText.text = $"{mission.type} {mission.missionTargetText}:";
                spawnUI.missionNowNumberText.text = "0";
                if (mission.type == missions.missionType.Annihilate)
                {
                    spawnUI.missionMaxNumberText.text = mission.killNumber.ToString();
                    spawnUI.missionNowNumberText.text = mission.killedNumber.ToString();
                }
                else if (mission.type == missions.missionType.Destroy)
                {
                    spawnUI.missionMaxNumberText.text = mission.destroyObject.Length.ToString();
                }
                else if (mission.type == missions.missionType.Boss)
                {
                    spawnUI.missionMaxNumberText.text = "1";
                }
                missionUis.Add(spawnUI);
            }

        }
    }

    public void UpdateMissionUi()
    {
        
    }

    public void DestroyAllUI()
    {
        foreach(MissionUI ui in missionUis)
        {
            Destroy(ui);
        }
        missionUis = new List<MissionUI>();
    }
}
