using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(missions))]
public class MissionEditor : Editor
{
    SerializedProperty typeProp;

    SerializedProperty bossPrefabProp;

    SerializedProperty destroyObjectProp;

    SerializedProperty timeLimitedProp;

    SerializedProperty killNumberProp;

    SerializedProperty missionScNameProp;

    SerializedProperty missionIDProp;

    SerializedProperty missionNamePorp;

    SerializedProperty missionDescribePorp;

    SerializedProperty missionAwardPorp;

    SerializedProperty missionTargetTextProp;

    private void OnEnable()
    {
        typeProp = serializedObject.FindProperty("type");
        bossPrefabProp = serializedObject.FindProperty("bossPrefab");
        destroyObjectProp = serializedObject.FindProperty("destroyObject");
        killNumberProp = serializedObject.FindProperty("killNumber");
        missionIDProp = serializedObject.FindProperty("missionID");
        missionNamePorp = serializedObject.FindProperty("missionName");
        timeLimitedProp = serializedObject.FindProperty("timeLimited");
        missionDescribePorp = serializedObject.FindProperty("missionDescribe");
        missionAwardPorp = serializedObject.FindProperty("missionAward");
        missionTargetTextProp = serializedObject.FindProperty("missionTargetText");
        missionScNameProp = serializedObject.FindProperty("missionSceneName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(missionIDProp);
        EditorGUILayout.PropertyField(missionNamePorp);
        EditorGUILayout.PropertyField(missionDescribePorp);
        EditorGUILayout.PropertyField(missionTargetTextProp);
        EditorGUILayout.PropertyField(missionAwardPorp);
        EditorGUILayout.PropertyField(missionScNameProp);

        var type = (missions.missionType)typeProp.intValue;
        switch (type)
        {
            case missions.missionType.Annihilate:
                EditorGUILayout.LabelField("Need Kill Number");
                EditorGUILayout.PropertyField(killNumberProp);
                break;
            case missions.missionType.Destroy:
                EditorGUILayout.LabelField("Need Destroy Object");
                EditorGUILayout.PropertyField(destroyObjectProp);
                EditorGUILayout.LabelField("Destory Time Limited");
                EditorGUILayout.PropertyField(timeLimitedProp);
                break;
            case missions.missionType.Boss:
                EditorGUILayout.LabelField("Boss");
                EditorGUILayout.PropertyField(bossPrefabProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
